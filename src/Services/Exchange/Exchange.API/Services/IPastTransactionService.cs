namespace Exchange.API.Services
{
    public interface IPastTransactionService
    {
        public Task<IEnumerable<PastTransaction>> GetPastTransactionsByUserIdAsync(int userId);
    }
}
