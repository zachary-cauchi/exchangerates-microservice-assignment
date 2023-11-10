namespace Exchange.API.Models
{
    public interface IUnitOfWork : IDisposable
    {
        public Task ExecuteTransactionAsync(Action transactionChanges, string message, CancellationToken cancellationToken);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
