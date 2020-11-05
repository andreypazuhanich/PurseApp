using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PurseApp.Models;

namespace PurseApp.Repositories
{
    public interface ICurrencyRepository
    {
        Task CreateCurrency(string name);
        Task<IEnumerable<Currency>> GetCurrencies();
        Task<Currency> GetCurrencyByName(string currencyName);
        Task<Currency> GetDefaultCurrency();
    }
}