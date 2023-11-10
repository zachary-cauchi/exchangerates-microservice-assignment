namespace Exchange.API.Services
{
    public class PastTransactionService : IPastTransactionService
    {
        private IPastTransactionRepository _repository;
        private ILogger<PastTransactionService> _logger;

        public PastTransactionService(IPastTransactionRepository repository, ILogger<PastTransactionService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<PastTransaction>> GetPastTransactionsByUserIdAsync(int userId)
        {
            _logger.LogInformation("Getting past transactions for user id ({id}).", userId);

            return await _repository.GetPastTransactionsByUserIdAsync(userId);
        }
    }
}
