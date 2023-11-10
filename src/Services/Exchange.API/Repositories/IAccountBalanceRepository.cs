namespace Exchange.API.Repositories
{
    public interface IAccountBalanceRepository : IRepository<AccountBalance>
    {
        public Task<AccountBalance?> GetAccountBalanceByIdAsync(int id);

        public void UpdateAccountBalance(AccountBalance accountBalance);
    }
}
