using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PurseApp.Models;

namespace PurseApp.Repositories
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAccounts(Guid purseId);
        Task<Account> GetAccount(Guid accountId);
        Task<Account> CreateAccount(Guid purseId,Currency currency, string name);
        Task<decimal> GetBalance(Guid accountId);
        Task AddBalance(Guid accountId, decimal amount);
        Task WithDrawMoney(Guid accountId, decimal amount);
        Task RemoveAccount(Guid accountId);
    }
}