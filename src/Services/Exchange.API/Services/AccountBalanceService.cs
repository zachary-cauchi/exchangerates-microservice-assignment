using Exchange.API.Data;
using Exchange.API.Models;
using Exchange.API.Repositories;
using Microsoft.EntityFrameworkCore;

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

        // This is temporary and will be replaced by a better implementation pattern.
        public async Task<AccountBalance> ExchangeCurrenciesAsync(int srcId, int destId, decimal srcAmount, decimal destAmount)
        {

            if (srcId == destId) throw new InvalidOperationException("Source and destination account balances cannot be the same.");

            var srcAccountBalance = await _repository.GetAccountBalanceByIdAsync(srcId);
            var destAccountBalance = await _repository.GetAccountBalanceByIdAsync(destId);

            if (srcAccountBalance == null) throw new InvalidOperationException($"Cannot find source account balance with id {srcId}");
            if (destAccountBalance == null) throw new InvalidOperationException($"Cannot find destination account balance with id {srcId}");

            if (srcAccountBalance.Balance < srcAmount) throw new InvalidOperationException("The desired amount to withdraw exceeds the source account balance.");
            if (srcAccountBalance.CurrencyId == destAccountBalance.CurrencyId) throw new InvalidOperationException("The source and destination account balances cannot have the same currency.");
            if (srcAccountBalance.UserId != destAccountBalance.UserId) throw new InvalidOperationException("The source and destination account balances must have the same user.");

            //TODO: Replace with actual transaction logic.
            destAccountBalance.Balance += destAmount;

            return destAccountBalance;
        }
    }
}
