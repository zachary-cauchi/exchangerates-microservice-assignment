namespace Exchange.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository repository, ILogger<UserService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            _logger.LogInformation("Getting user with id ({id}).", id);

            return await _repository.GetUserByIdAsync(id);
        }
    }
}
