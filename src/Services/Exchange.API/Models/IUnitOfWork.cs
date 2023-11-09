using Microsoft.EntityFrameworkCore.Storage;

namespace Exchange.API.Models
{
    public interface IUnitOfWork : IDisposable
    {
        public Task ExecuteTransactionAsync(Action transactionChanges, string message);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
