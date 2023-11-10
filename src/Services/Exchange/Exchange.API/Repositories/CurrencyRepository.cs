namespace Exchange.API.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly ExchangeAPIContext _context;

        public CurrencyRepository(ExchangeAPIContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<Currency?> GetCurrencyByIdAsync(int id)
        {
            var currency = await _context.Currency.FirstOrDefaultAsync(c => c.Id == id);

            if (currency == null)
            {
                currency = _context.Currency.Local.FirstOrDefault(c => c.Id == id);
            }

            return currency;
        }

        private bool CurrencyExists(int id)
        {
            return (_context.Currency?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
