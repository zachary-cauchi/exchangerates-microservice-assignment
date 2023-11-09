using Exchange.API.Models;

namespace Exchange.API.Repositories
{
    public interface IAccountBalanceRepository : IRepository<AccountBalance>
    {
        public Task<AccountBalance?> GetAccountBalanceByIdAsync(int id);
    }
}
