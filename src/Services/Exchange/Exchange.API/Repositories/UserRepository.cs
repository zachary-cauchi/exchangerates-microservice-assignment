namespace Exchange.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ExchangeAPIContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(ExchangeAPIContext context, ILogger<UserRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<User?> GetUserByIdAsync(int id)
        {
            _logger.LogDebug("Getting user with id ({id}).", id);

            return await _context.Users
                .Include(u => u.AccountBalances)
                .FirstOrDefaultAsync(u => u.Id == id); ;
        }
    }
}
