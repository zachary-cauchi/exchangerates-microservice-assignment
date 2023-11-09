﻿using Exchange.API.Models;

namespace Exchange.API.Services
{
    public interface IAccountBalanceService
    {
        public Task<AccountBalance?> GetAccountBalanceByIdAsync(int id);
    }
}
