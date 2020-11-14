using System;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using PurseApp.CurrencyIntegration;
using PurseApp.Repositories;

namespace PurseApp.Hangfire
{
    public class TransactionWorker : IWorker
    {
        private readonly PurseAppDbContext _purseAppDbContext;
        private readonly IAccountRepository _accountRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ICurrencyApi _currencyApi;
        
        public TransactionWorker(PurseAppDbContext purseAppDbContext, IAccountRepository accountRepository, ICurrencyRepository currencyRepository, ICurrencyApi currencyApi)
        {
            _purseAppDbContext = purseAppDbContext;
            _accountRepository = accountRepository;
            _currencyRepository = currencyRepository;
            _currencyApi = currencyApi;
        }
        
        [AutomaticRetry(Attempts = 0, DelaysInSeconds=new int[] { 300 })]
        public async Task Run()
        {
            var transactionsToProcess = await _purseAppDbContext.Transactions
                .Where(s => s.Status == TransactionStatus.Created).ToListAsync();
            foreach (var transaction in transactionsToProcess)
            {
                try
                {
                    await TransferMoney(transaction.AccountSourceId, transaction.AccountDestinationId,
                        transaction.Amount);
                    transaction.Status = TransactionStatus.Successed;
                }
                catch (Exception)
                {
                    transaction.Status = TransactionStatus.Failed;
                    throw;
                }
                finally
                {
                    await _purseAppDbContext.SaveChangesAsync();
                }
            }
        }
        
        public async Task TransferMoney(Guid accountIdSource, Guid accountIdDest, decimal amount)
        {
            //INFO: валюта amount = валюте sourceAccount
            if (amount <= 0)
               throw new Exception("Сумма перевода должна быть больше нуля");
            
            var accountSource = await _accountRepository.GetAccount(accountIdSource);
            if (accountSource == null)
                throw new Exception("Счет для списания денежных средств не существует");
            
            if (accountSource.Balance < amount)
                throw new Exception("Недостаточно денежных средств для перевода");
            
            var accountDestination = await _accountRepository.GetAccount(accountIdDest);
            if (accountDestination == null)
                throw new Exception("Счет назначения перевода ДС не существует");

            if (accountSource.PurseId != accountDestination.PurseId)
                throw new Exception("Переводы валюты разрешены в рамках одного кошелька");
            
            var currenciesData = await _currencyApi.GetApi();
            if (currenciesData == null)
                throw new Exception("Не найдены курсы валют");
            
            ValCursValute sourceAccountCurrencyInfo = null;
            if (!(accountSource.Currency.CurrencyId == (await _currencyRepository.GetDefaultCurrency()).CurrencyId))
            {
                sourceAccountCurrencyInfo = currenciesData.Valute
                    .FirstOrDefault(s => s.CharCode == accountSource.Currency.Name);
                if(sourceAccountCurrencyInfo == null)
                    throw new Exception("Не найден курс валют для исходного счета");
            }
                
            ValCursValute destAccountCurrencyInfo = null;
            if (!(accountDestination.Currency.CurrencyId == (await _currencyRepository.GetDefaultCurrency()).CurrencyId))
            {
                destAccountCurrencyInfo = currenciesData.Valute
                    .FirstOrDefault(s => s.CharCode == accountDestination.Currency.Name);
                if ( destAccountCurrencyInfo == null)
                    throw new Exception("Не найден курс валют счета назначения");
            }
            await _accountRepository.WithDrawMoney(accountSource.AccountId, amount);
            var amountDest = CalculateAmountDestination(sourceAccountCurrencyInfo, destAccountCurrencyInfo, amount);
            await _accountRepository.AddBalance(accountDestination.AccountId,amountDest);
        }
        private decimal CalculateAmountDestination(ValCursValute sourceAccountCurrencyInfo, ValCursValute destAccountCurrencyInfo, decimal amount)
        {
            var sourceAccRate = 1M;// Для случая с дефолтным счетом(рублевым)
            var sourceNominal = 1;
            if (sourceAccountCurrencyInfo != null)
            {
                sourceAccRate = Convert.ToDecimal(sourceAccountCurrencyInfo.Value);
                sourceNominal = Convert.ToInt32(sourceAccountCurrencyInfo.Nominal);
            }
            
            var destAccRate = 1M;// Для случая с дефолтным счетом(рублевым)
            var destNominal = 1;
            if (destAccountCurrencyInfo != null)
            {
                destAccRate = Convert.ToDecimal(destAccountCurrencyInfo.Value );
                destNominal = Convert.ToInt32(destAccountCurrencyInfo.Nominal);
            }
            return amount * sourceAccRate / sourceNominal / destAccRate * destNominal ;
        }
    }
}