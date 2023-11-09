using Exchange.API.Models;

namespace Exchange.API.Services
{
    public interface IUserService
    {
        public Task<User?> GetUserByIdAsync(int id);
    }
}
