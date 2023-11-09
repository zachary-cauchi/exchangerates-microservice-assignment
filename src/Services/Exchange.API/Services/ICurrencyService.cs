using Exchange.API.Models;

namespace Exchange.API.Services
{
    public interface ICurrencyService
    {
        public Task<Currency?> GetCurrencyByIdAsync(int id);
    }
}
