namespace Exchange.API.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly ExchangeAPIContext _context;
        private readonly ILogger<CurrencyRepository> _logger;

        public CurrencyRepository(ExchangeAPIContext context, ILogger<CurrencyRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<Currency?> GetCurrencyByIdAsync(int id)
        {
            _logger.LogDebug("Getting currency with id ({id})", id);

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
