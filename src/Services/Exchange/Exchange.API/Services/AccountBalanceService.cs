namespace Exchange.API.Services
{
    public class AccountBalanceService : IAccountBalanceService
    {
        private IAccountBalanceRepository _repository;
        private ILogger<AccountBalanceService> _logger;

        public AccountBalanceService(IAccountBalanceRepository repository, ILogger<AccountBalanceService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(_logger));
        }

        public async Task<AccountBalance?> GetAccountBalanceByIdAsync(int id)
        {
            _logger.LogInformation("Getting account balance with id ({id})", id);

            return await _repository.GetAccountBalanceByIdAsync(id);
        }
    }
}
