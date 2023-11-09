using Exchange.API.Models;

namespace Exchange.API.Repositories
{
    public interface IPastTransactionRepository : IRepository<PastTransaction>
    {
        public PastTransaction Add(PastTransaction pastTransaction);
        public Task<IEnumerable<PastTransaction>> GetPastTransactionsByUserIdAsync(int userId);
    }
}
