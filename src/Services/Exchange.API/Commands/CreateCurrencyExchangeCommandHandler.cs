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

            if (srcId == destId) throw new InvalidOperationException("Source and destination account balances cannot be the same.");

            var srcAccountBalance = await _accountBalanceRepository.GetAccountBalanceByIdAsync(srcId);
            var destAccountBalance = await _accountBalanceRepository.GetAccountBalanceByIdAsync(destId);

            if (srcAccountBalance == null) throw new InvalidOperationException($"Cannot find source account balance with id {srcId}");
            if (destAccountBalance == null) throw new InvalidOperationException($"Cannot find destination account balance with id {srcId}");

            int srcCurrencyId = srcAccountBalance.CurrencyId;
            int destCurrencyId = destAccountBalance.CurrencyId;

            Currency? srcCurrency = await _currencyRepository.GetCurrencyByIdAsync(srcCurrencyId);
            Currency? destCurrency = await _currencyRepository.GetCurrencyByIdAsync(destCurrencyId);

            if (srcCurrency == null) throw new InvalidOperationException($"The source currency ({srcCurrencyId}) for source account '{srcAccountBalance.Id}' does not exist. Cannot proceed.");
            if (destCurrency == null) throw new InvalidOperationException($"The destination currency ({destCurrencyId}) for destination account '{destAccountBalance.Id}' does not exist. Cannot proceed.");

            if (srcAccountBalance.Balance < debitAmount) throw new InvalidOperationException("The desired amount to withdraw exceeds the source account balance.");
            if (srcAccountBalance.CurrencyId == destAccountBalance.CurrencyId) throw new InvalidOperationException("The source and destination account balances cannot have the same currency.");
            if (srcAccountBalance.UserId != destAccountBalance.UserId) throw new InvalidOperationException("The source and destination account balances must have the same user.");

            User? user = await _userRepository.GetUserByIdAsync(srcAccountBalance.UserId);

            if (user == null) throw new InvalidOperationException($"User ({srcAccountBalance.UserId}) does not exist. Cannot proceed.");

            await _accountBalanceRepository.UnitOfWork.ExecuteTransactionAsync(() =>
            {
                destAccountBalance.Balance += creditAmount;
                srcAccountBalance.Balance -= debitAmount;
                _accountBalanceRepository.UpdateAccountBalance(destAccountBalance);
            }, "CreateCurrencyExchangeCommandHandler", cancellationToken);

            PastTransaction pastTransaction = new PastTransaction(user, srcAccountBalance, destAccountBalance, debitAmount, DateTime.UtcNow, srcCurrency, destCurrency, exchangeRate);
            await _pastTransactionRepository.UnitOfWork.ExecuteTransactionAsync(() =>
            {
                _pastTransactionRepository.Add(pastTransaction);
            }, "CreateCurrencyExchangeCommandHandler", cancellationToken);

            return pastTransaction;
        }
    }
}
