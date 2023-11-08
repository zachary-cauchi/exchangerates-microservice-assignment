using Exchange.API.Data;
using Exchange.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Exchange.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ExchangeAPIContext _context;

        public UserRepository(ExchangeAPIContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
