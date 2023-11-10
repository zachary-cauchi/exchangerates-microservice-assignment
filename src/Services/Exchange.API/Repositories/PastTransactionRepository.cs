namespace Exchange.API.Repositories
{
    public class PastTransactionRepository : IPastTransactionRepository
    {
        private readonly ExchangeAPIContext _context;

        public PastTransactionRepository(ExchangeAPIContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitOfWork UnitOfWork => _context;

        public PastTransaction Add(PastTransaction pastTransaction)
        {
            return _context.PastTransactions.Add(pastTransaction).Entity;
        }

        public async Task<IEnumerable<PastTransaction>> GetPastTransactionsByUserIdAsync(int userId)
        {
            return await _context.PastTransactions
                .Where(p => p.UserId == userId)
                .Include(p => p.User)
                .Include(p => p.FromCurrency)
                .Include(p => p.ToCurrency)
                .ToListAsync();
        }
    }
}
