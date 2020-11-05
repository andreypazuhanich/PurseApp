using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PurseApp.CurrencyIntegration;
using PurseApp.Models;
using PurseApp.Repositories;

namespace PurseApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IPurseRepository _purseRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ICurrencyApi _currencyApi;

        public AccountController(IAccountRepository accountRepository, IPurseRepository purseRepository, ICurrencyRepository currencyRepository, ICurrencyApi currencyApi)
        {
            _accountRepository = accountRepository;
            _purseRepository = purseRepository;
            _currencyRepository = currencyRepository;
            _currencyApi = currencyApi;
        }

        [HttpGet("{accountId}")]
        public async Task<ActionResult<Account>> GetAccountById(Guid accountId)
        {
            var account = await _accountRepository.GetAccount(accountId);
            if (account == null)
                return NoContent();
            return Ok(account);
        }

        [HttpGet("accounts/{purseId}")]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts(Guid purseId)
        {
            if ( await _purseRepository.GetPurse(purseId) == null)
                return BadRequest($"Кошелек с id = {purseId} не найден");
            return Ok(await _accountRepository.GetAccounts(purseId));
        } 

        [HttpPut]
        public async Task<IActionResult> CreateAccount(Guid purseId,string currencyName,string accountName)
        {
            var purse = await _purseRepository.GetPurse(purseId); 
            if(purse.Accounts.Any(s => s.Currency.Name == currencyName))
                throw new Exception("Счет в такой валюте уже существует");
            
            var account = await _accountRepository.CreateAccount(purseId, new Currency {Name = currencyName}, accountName);

            return Ok();
        }  
        
        [HttpPost("addmoney/{accountId}")]
        public async Task<IActionResult> AddBalance([FromRoute]Guid accountId, [FromQuery] decimal amount)
        {
            try
            {
                await _accountRepository.AddBalance(accountId, amount);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }
        
        [HttpPost("withdrawmoney/{accountId}")]
        public async Task<IActionResult> WithDrawBalance([FromRoute]Guid accountId, [FromQuery] decimal amount)
        {
            try
            {
                await _accountRepository.WithDrawMoney(accountId, amount);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
        
        [HttpPost("transfermoney")]
        public async Task<IActionResult> TransferMoney([FromQuery]Guid accountIdSource, [FromQuery]Guid accountIdDest,[FromQuery]decimal amount)
        {
            //INFO: валюта amount = валюте sourceAccount
            if (amount <= 0)
                return BadRequest("Сумма перевода должна быть больше нуля");
            
            var accountSource = await _accountRepository.GetAccount(accountIdSource);
            if (accountSource == null)
                return BadRequest("Счет для списания денежных средств не существует");
            
            if (accountSource.Balance < amount)
                return BadRequest("Недостаточно денежных средств для перевода");
            
            var accountDestination = await _accountRepository.GetAccount(accountIdDest);
            if (accountDestination == null)
                return BadRequest("Счет назначения перевода ДС не существует");
            
            var currenciesData = await _currencyApi.GetApi();
            if (currenciesData == null)
                return BadRequest("Не найдены курсы валют");
            
            ValCursValute sourceAccountCurrencyInfo = null;
            if (!(accountSource.Currency.CurrencyId == (await _currencyRepository.GetDefaultCurrency()).CurrencyId))
            {
                sourceAccountCurrencyInfo = currenciesData.Valute
                    .FirstOrDefault(s => s.CharCode == accountSource.Currency.Name);
                if(sourceAccountCurrencyInfo == null)
                    return BadRequest("Не найден курс валют для исходного счета");
            }
                
            ValCursValute destAccountCurrencyInfo = null;
            if (!(accountDestination.Currency.CurrencyId == (await _currencyRepository.GetDefaultCurrency()).CurrencyId))
            {
                destAccountCurrencyInfo = currenciesData.Valute
                    .FirstOrDefault(s => s.CharCode == accountDestination.Currency.Name);
                if ( destAccountCurrencyInfo == null)
                    return BadRequest("Не найден курс валют счета назначения");
            }

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
            await _accountRepository.WithDrawMoney(accountSource.AccountId, amount);
            var amountDest = amount * sourceAccRate / sourceNominal / destAccRate * destNominal ;
            await _accountRepository.AddBalance(accountDestination.AccountId,amountDest);
            
            return Ok();
        }
        
        [HttpDelete("delete/{accountId}")]
        public async Task<IActionResult> RemoveAccount(Guid accountId)
        {
            await _accountRepository.RemoveAccount(accountId);
            return Ok();
        }
    }
}