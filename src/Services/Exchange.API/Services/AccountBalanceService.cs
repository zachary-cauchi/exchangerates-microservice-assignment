using Microsoft.EntityFrameworkCore.Storage;

namespace Exchange.API.Services
{
    public class AccountBalanceService : IAccountBalanceService
    {
        private IAccountBalanceRepository _repository;

        public AccountBalanceService(IAccountBalanceRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<AccountBalance?> GetAccountBalanceByIdAsync(int id)
        {
            return await _repository.GetAccountBalanceByIdAsync(id);
        }
    }
}
