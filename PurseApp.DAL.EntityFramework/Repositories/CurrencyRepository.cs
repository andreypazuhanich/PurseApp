using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PurseApp.Models;

namespace PurseApp.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly PurseAppDbContext _dbContext;

        public CurrencyRepository(PurseAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task CreateCurrency(string name)
        {
            await _dbContext.Currencies.AddAsync(new Currency {Name = name});
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Currency>> GetCurrencies()
        {
          return await _dbContext.Currencies.ToListAsync();
        }

        public async Task<Currency> GetCurrencyByName(string currencyName)
        {
            return await _dbContext.Currencies.FirstOrDefaultAsync(s =>
                s.Name.Equals(currencyName, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<Currency> GetDefaultCurrency()
        {
            return await _dbContext.Currencies.FirstOrDefaultAsync(s => s.Name == "RUB");
        }
    }
}