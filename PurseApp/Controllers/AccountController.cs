using System;
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
        private readonly PurseAppDbContext _dbContext;

        public AccountController(IAccountRepository accountRepository, IPurseRepository purseRepository, ICurrencyRepository currencyRepository, ICurrencyApi currencyApi, PurseAppDbContext dbContext)
        {
            _accountRepository = accountRepository;
            _purseRepository = purseRepository;
            _currencyRepository = currencyRepository;
            _currencyApi = currencyApi;
            _dbContext = dbContext;
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

        [HttpPut("create/{purseId}")]
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
        
        [HttpPost("transfermoney/{accountIdSource}")]
        public async Task<IActionResult> TransferMoney(Guid accountIdSource, [FromQuery]Guid accountIdDest,[FromQuery]decimal amount)
        {
            var transaction = new Transaction
            {
                AccountSourceId = accountIdSource,
                AccountDestinationId = accountIdDest,
                Amount = amount
            };
            await _dbContext.Transactions.AddAsync(transaction);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
        
        [HttpDelete("delete/{purseId}/{accountId}")]
        public async Task<IActionResult> RemoveAccount([FromRoute]Guid purseId, [FromRoute]Guid accountId)
        {
            if (!await _purseRepository.IsPurseExists(purseId))
                return NotFound();
            await _accountRepository.RemoveAccount(accountId);
            return Ok();
        }
        
    }
}