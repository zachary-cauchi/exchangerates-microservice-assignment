namespace Exchange.API.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<User?> GetUserByIdAsync(int id);
    }
}
