namespace Exchange.API.Repositories
{
    public class AccountBalanceRepository : IAccountBalanceRepository
    {
        private readonly ExchangeAPIContext _context;
        private readonly ILogger<AccountBalanceRepository> _logger;

        public AccountBalanceRepository(ExchangeAPIContext context, ILogger<AccountBalanceRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<AccountBalance?> GetAccountBalanceByIdAsync(int id)
        {
            _logger.LogDebug("Getting account balance ({id})", id);

            var accountBalance = await _context.AccountBalances
                .FirstOrDefaultAsync(c => c.Id == id);

            if (accountBalance == null)
            {
                accountBalance = _context.AccountBalances.Local
                    .FirstOrDefault(c => c.Id == id);
            }

            return accountBalance;
        }

        public void UpdateAccountBalance(AccountBalance accountBalance)
        {
            _logger.LogDebug("Updating account balance ({id})", accountBalance.Id);

            _context.Entry(accountBalance).State = EntityState.Modified;
        }
    }
}
