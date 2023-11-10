namespace Exchange.API.Repositories
{
    public class PastTransactionRepository : IPastTransactionRepository
    {
        private readonly ExchangeAPIContext _context;
        private readonly ILogger<PastTransactionRepository> _logger;

        public PastTransactionRepository(ExchangeAPIContext context, ILogger<PastTransactionRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IUnitOfWork UnitOfWork => _context;

        public PastTransaction Add(PastTransaction pastTransaction)
        {
            _logger.LogDebug("Adding pastTransaction with timestamp ({timestamp}).", pastTransaction.TimeEffected);
            
            return _context.PastTransactions.Add(pastTransaction).Entity;
        }

        public async Task<IEnumerable<PastTransaction>> GetPastTransactionsByUserIdAsync(int userId)
        {
            _logger.LogDebug("Getting past transactions for user id ({id})", userId);
            
            return await _context.PastTransactions
                .Where(p => p.UserId == userId)
                .Include(p => p.User)
                .Include(p => p.FromCurrency)
                .Include(p => p.ToCurrency)
                .ToListAsync();
        }

        public async Task<IEnumerable<PastTransaction>> GetPastTransactionsByUserSinceDateTimeAsync(int userId, DateTime threshold)
        {
            _logger.LogDebug("Getting past transactions for user id ({userId}) since timestamp ({threshold})", userId, threshold);
            
            return await _context.PastTransactions
                .Where(p => p.UserId == userId)
                .Where(p => p.TimeEffected > threshold)
                .Include(p => p.User)
                .Include(p => p.FromCurrency)
                .Include(p => p.ToCurrency)
                .ToListAsync();
        }

        public async Task<int> GetPastTransactionsCountByUserSinceDateTimeAsync(int userId, DateTime threshold)
        {
            _logger.LogDebug("Getting past transactions count for user id ({userId}) since timestamp ({threshold})", userId, threshold);

            return await _context.PastTransactions
                .Where(p => p.UserId == userId)
                .Where(p => p.TimeEffected > threshold)
                .CountAsync();
        }
    }
}
