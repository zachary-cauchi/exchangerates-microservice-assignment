using Exchange.API.Models;


namespace Exchange.API.Services
{
    public class PastTransactionService : IPastTransactionService
    {
        private IPastTransactionRepository _repository;

        public PastTransactionService(IPastTransactionRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<PastTransaction>> GetPastTransactionsByUserIdAsync(int userId)
        {
            return await _repository.GetPastTransactionsByUserIdAsync(userId);
        }
    }
}
