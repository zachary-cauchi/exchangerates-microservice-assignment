namespace Exchange.API.Commands
{
    public class CreateCurrencyExchangeCommandHandler : IRequestHandler<CreateCurrencyExchangeCommand, PastTransaction>
    {
        private readonly ExchangeAPIContext _context;
        private readonly IAccountBalanceRepository _accountBalanceRepository;
        private readonly IPastTransactionRepository _pastTransactionRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IMediator _mediator;
        private readonly ILogger<CreateCurrencyExchangeCommandHandler> _logger;

        private readonly int hoursThreshold = 1;
        private readonly int maxTransactionsPerTimeframe = 10;

        public CreateCurrencyExchangeCommandHandler(ExchangeAPIContext context, IAccountBalanceRepository accountBalanceRepository, IPastTransactionRepository pastTransactionRepository, IUserRepository userRepository, ICurrencyRepository currencyRepository, IMediator mediator, ILogger<CreateCurrencyExchangeCommandHandler> logger)
        {
            _context = context;
            _accountBalanceRepository = accountBalanceRepository ?? throw new ArgumentNullException(nameof(accountBalanceRepository));
            _pastTransactionRepository = pastTransactionRepository ?? throw new ArgumentNullException(nameof(pastTransactionRepository));
            _userRepository = userRepository;
            _currencyRepository = currencyRepository;
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PastTransaction> Handle(CreateCurrencyExchangeCommand message, CancellationToken cancellationToken)
        {
            int srcId = message.FromAccountBalanceId;
            int destId = message.ToAccountBalanceId;
            decimal debitAmount = message.DebitAmount;
            decimal exchangeRate = message.ExchangeRate;
            decimal creditAmount = debitAmount * exchangeRate;
            DateTime timeOfRate = message.TimeOfRate;

            if (srcId == destId)
            {
                _logger.LogWarning("Source and destination account balances are the same ({srcAcountBalanceId} - {destAcountBalanceId}), cannot proceed.", srcId, destId);

                throw new InvalidOperationException("Source and destination account balances cannot be the same.");
            }

            var srcAccountBalance = await _accountBalanceRepository.GetAccountBalanceByIdAsync(srcId);
            var destAccountBalance = await _accountBalanceRepository.GetAccountBalanceByIdAsync(destId);

            if (srcAccountBalance == null)
            {
                _logger.LogWarning("Could not find source account balance with id ({srcAccountBalanceId})", srcId);

                throw new InvalidOperationException($"Cannot find source account balance with id {srcId}");
            }

            if (destAccountBalance == null)
            {
                _logger.LogWarning("Could not find destination account balance with id ({destAccountBalanceId})", destId);

                throw new InvalidOperationException($"Cannot find destination account balance with id {destId}");
            }

            int srcCurrencyId = srcAccountBalance.CurrencyId;
            int destCurrencyId = destAccountBalance.CurrencyId;

            Currency? srcCurrency = await _currencyRepository.GetCurrencyByIdAsync(srcCurrencyId);
            Currency? destCurrency = await _currencyRepository.GetCurrencyByIdAsync(destCurrencyId);

            if (srcCurrency == null)
            {
                _logger.LogWarning("Could not find source currency ({srcCurrencyId})", srcCurrencyId);

                throw new InvalidOperationException($"The source currency ({srcCurrencyId}) for source account '{srcAccountBalance.Id}' does not exist. Cannot proceed.");
            }

            if (destCurrency == null)
            {
                _logger.LogWarning("Could not find destination currency ({destCurrencyId})", destCurrencyId);

                throw new InvalidOperationException($"The destination currency ({destCurrencyId}) for destination account '{destAccountBalance.Id}' does not exist. Cannot proceed.");
            }

            if (srcAccountBalance.Balance < debitAmount)
            {
                _logger.LogWarning("Amount to be debitted ({debitAmount}) for source account id ({srcCurrencyId}) exceeds total balance, cannot continue.", debitAmount, srcCurrencyId);

                throw new InvalidOperationException("The desired amount to withdraw exceeds the source account balance.");
            }
            if (srcAccountBalance.CurrencyId == destAccountBalance.CurrencyId)
            {
                _logger.LogWarning("Source and destination account balances use the same currency ({srcCurrencyId}), cannot continue.", srcCurrencyId);

                throw new InvalidOperationException("The source and destination account balances cannot have the same currency.");
            }

            if (srcAccountBalance.UserId != destAccountBalance.UserId)
            {
                _logger.LogWarning("Source account balance belongs to user ({srcUserId}) while destination account balance belongs to user ({destUserId}), cannot proceed.", srcAccountBalance.UserId, destAccountBalance.UserId);

                throw new InvalidOperationException("The source and destination account balances must have the same user.");
            }

            User? user = await _userRepository.GetUserByIdAsync(srcAccountBalance.UserId);

            if (user == null)
            {
                _logger.LogWarning("User with id ({userId}) was not found.", srcAccountBalance.UserId);

                throw new InvalidOperationException($"User ({srcAccountBalance.UserId}) does not exist. Cannot proceed.");
            }

            var threshold = DateTime.UtcNow.AddHours(hoursThreshold * -1);
            int transactionsPastHourCount = await _pastTransactionRepository.GetPastTransactionsCountByUserSinceDateTimeAsync(user.Id, threshold);

            if (transactionsPastHourCount >= maxTransactionsPerTimeframe)
            {
                _logger.LogWarning("User with id ({userId}) exceeded transaction limit: {transactionCount} since {timeThreshold}", srcAccountBalance.UserId, transactionsPastHourCount, threshold);

                throw new InvalidOperationException($"The user exceeded the number of transactions that can be done in the past ({hoursThreshold}) hour(s); Only ({transactionsPastHourCount}) transactions are allowed.");
            }

            _logger.LogInformation("Validations for currency exchange passed. Performing transactions.");

            await _accountBalanceRepository.UnitOfWork.ExecuteTransactionAsync(() =>
            {
                _logger.LogDebug("Debitting account ({srcAccountBalanceId}) the amount of ({debitAmount}).", srcAccountBalance.Id, debitAmount);
                srcAccountBalance.Balance -= debitAmount;

                _logger.LogDebug("Creditting account ({destAccountBalanceId}) the amount of ({creditAmount}).", destAccountBalance.Id, creditAmount);
                destAccountBalance.Balance += creditAmount;

                _logger.LogDebug("Updating accounts on database.");
                _accountBalanceRepository.UpdateAccountBalance(srcAccountBalance);
                _accountBalanceRepository.UpdateAccountBalance(destAccountBalance);
            }, "CreateCurrencyExchangeCommandHandler", cancellationToken);

            _logger.LogInformation("Accounts updated. Saving transaction information.");

            PastTransaction pastTransaction = new PastTransaction(user, srcAccountBalance, destAccountBalance, debitAmount, creditAmount, DateTime.UtcNow, srcCurrency, destCurrency, exchangeRate);
            await _pastTransactionRepository.UnitOfWork.ExecuteTransactionAsync(() =>
            {
                pastTransaction = _pastTransactionRepository.Add(pastTransaction);

                _logger.LogInformation("Created past transaction ({pastTransactionId})", pastTransaction.Id);
            }, "CreateCurrencyExchangeCommandHandler", cancellationToken);

            _logger.LogInformation("Currency exchange between account balances ({srcAccountBalanceId}) and ({destAccountBalanceId}) completed.", srcId, destId);

            return pastTransaction;
        }
    }
}
