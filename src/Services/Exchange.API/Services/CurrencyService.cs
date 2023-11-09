using Exchange.API.Models;


namespace Exchange.API.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _repository;

        public CurrencyService(ICurrencyRepository repository)
        {
            _repository = repository;
        }

        public async Task<Currency?> GetCurrencyByIdAsync(int id)
        {
            return await _repository.GetCurrencyByIdAsync(id);
        }
    }
}
