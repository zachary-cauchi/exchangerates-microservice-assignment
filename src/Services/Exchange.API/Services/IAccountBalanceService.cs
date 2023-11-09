using Exchange.API.Models;

namespace Exchange.API.Services
{
    public interface IAccountBalanceService
    {
        public Task<AccountBalance?> GetAccountBalanceByIdAsync(int id);

        // This is temporary and will be replaced by a better implementation pattern.
        public Task<AccountBalance> ExchangeCurrenciesAsync(int srcId, int destId, decimal srcAmount, decimal destAmount);
    }
}
