using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using PurseApp.Models;

namespace PurseApp.Repositories
{
    public class AccountRepository : IAccountRepository
    {

        private readonly PurseAppDbContext _dbContext;

        public AccountRepository(PurseAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<IEnumerable<Account>> GetAccounts(Guid purseId)
        {
            return await _dbContext.Accounts.Where(s => s.PurseId == purseId).Include(s => s.Currency).ToListAsync();
        }

        public async Task<Account> GetAccount(Guid accountId)
        {
            return await _dbContext.Accounts.Include(s => s.Currency)
                .FirstOrDefaultAsync(s => s.AccountId == accountId);
        }

        public async Task<Account> CreateAccount(Guid purseId, Currency currency, string name)
        {
            if (await _dbContext.Accounts.AnyAsync(s => s.PurseId == purseId && s.Currency.CurrencyId == currency.CurrencyId))
                throw new Exception("Счет с такой валютой уже существует");
            var account = new Account {PurseId = purseId, Currency = currency, Name = name}; 
            
            await _dbContext.Accounts.AddAsync(account);
            await _dbContext.SaveChangesAsync();
            
            return account;
        }

        public async Task<decimal> GetBalance(Guid accountId)
        {
            var account = await _dbContext.Accounts.FirstOrDefaultAsync(s => s.AccountId == accountId);
            return account.Balance;
        }

        public async Task AddBalance(Guid accountId, decimal amount)
        {
            var account = await _dbContext.Accounts.FirstOrDefaultAsync(s => s.AccountId == accountId);
            if(account == null)
                throw new Exception("Такого счета не существует");
            account.Balance += amount;
            await _dbContext.SaveChangesAsync();
        }

        public async Task WithDrawMoney(Guid accountId, decimal amount)
        {
            var account = await _dbContext.Accounts.FirstOrDefaultAsync(s => s.AccountId == accountId);
            if(account.Balance - amount < 0M)
                throw new Exception("Недостаточно денежных средств");
            account.Balance -= amount;
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveAccount(Guid accountId)
        {
            var account = await _dbContext.Accounts.FirstOrDefaultAsync(s => s.AccountId == accountId);
            if (account != null)
            {
                _dbContext.Accounts.Remove(account);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}