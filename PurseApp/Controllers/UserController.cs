using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PurseApp.Models;
using PurseApp.Repositories;

namespace PurseApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserRepository _userRepository;
        private readonly IPurseRepository _purseRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ICurrencyRepository _currencyRepository;

        private const string DefaultAccountName = "Рублевый счет";
        
        public UserController(IUserRepository userRepository, IPurseRepository purseRepository, IAccountRepository accountRepository, ICurrencyRepository currencyRepository)
        {
            _userRepository = userRepository;
            _purseRepository = purseRepository;
            _accountRepository = accountRepository;
            _currencyRepository = currencyRepository;
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromQuery]string userName, [FromQuery]string inn)
        {
            var user = await _userRepository.CreateUser(userName, inn);
            if (user.Purses == null || !user.Purses.Any())
            {
                var purse =  await _purseRepository.CreatePurse(user.UserId);
                var currency = await _currencyRepository.GetDefaultCurrency();
                await _accountRepository.CreateAccount(purse.PurseId, currency, DefaultAccountName);
            }
            return Ok(user);
        }
    }
}