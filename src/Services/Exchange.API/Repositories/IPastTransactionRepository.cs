namespace Exchange.API.Repositories
{
    public interface IPastTransactionRepository : IRepository<PastTransaction>
    {
        public PastTransaction Add(PastTransaction pastTransaction);
        public Task<IEnumerable<PastTransaction>> GetPastTransactionsByUserIdAsync(int userId);
        public Task<IEnumerable<PastTransaction>> GetPastTransactionsByUserSinceDateTimeAsync(int userId, DateTime threshold);
        public Task<int> GetPastTransactionsCountByUserSinceDateTimeAsync(int userId, DateTime threshold);
    }
}
