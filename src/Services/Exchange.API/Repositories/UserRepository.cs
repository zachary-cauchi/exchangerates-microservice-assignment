namespace Exchange.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ExchangeAPIContext _context;

        public UserRepository(ExchangeAPIContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<User?> GetUserByIdAsync(int id)
        {
            var user = await _context.Users
                .Include(u => u.AccountBalances)
                .FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        //public async Task CreateAccountBalanceForUser(int userId, int currencyId)
        //{

        //}
    }
}
