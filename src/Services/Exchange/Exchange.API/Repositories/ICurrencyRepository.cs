namespace Exchange.API.Repositories
{
    public interface ICurrencyRepository : IRepository<Currency>
    {

        public Task<Currency?> GetCurrencyByIdAsync(int id);
    }
}
