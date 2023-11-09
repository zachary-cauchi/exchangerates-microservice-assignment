using Exchange.API.Data;
using Exchange.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Exchange.API.Repositories
{
    public class AccountBalanceRepository : IAccountBalanceRepository
    {
        private readonly ExchangeAPIContext _context;

        public AccountBalanceRepository(ExchangeAPIContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<AccountBalance?> GetAccountBalanceByIdAsync(int id)
        {
            var accountBalance = await _context.AccountBalances.FirstOrDefaultAsync(c => c.Id == id);

            if (accountBalance == null)
            {
                accountBalance = _context.AccountBalances.Local.FirstOrDefault(c => c.Id == id);
            }

            return accountBalance;
        }

        public void UpdateAccountBalance(AccountBalance accountBalance)
        {
            _context.Entry(accountBalance).State = EntityState.Modified;
        }
    }
}
