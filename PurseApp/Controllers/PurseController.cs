using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PurseApp.Models;
using PurseApp.Repositories;

namespace PurseApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurseController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPurseRepository _purseRepository;
        
        public PurseController(IUserRepository userRepository, IPurseRepository purseRepository)
        {
            _userRepository = userRepository;
            _purseRepository = purseRepository;
        }
        
        [HttpGet("purse/{userId}")]
        public async Task<ActionResult<IEnumerable<Purse>>> GetPurses(Guid userId)
        {
            if (! await _userRepository.IsUserExists(userId))
                return NotFound("Пользователь не существует");
            return Ok(await _purseRepository.GetPurses(userId));
        }
        
        [HttpGet("purse/accounts/{userId}")]
        public async Task<ActionResult<IEnumerable<(string,string,string)>>> GetPursesBalance(Guid userId)
        {
            if (! await _userRepository.IsUserExists(userId))
                return NotFound("Пользователь не существует");
            var purses = (await _purseRepository.GetPurses(userId)).SelectMany(s => s.Accounts)
                .Select(s => new {accountname = s.Name, s.Balance, currencyname = s.Currency.Name});
            return Ok(purses);
        }

        [HttpDelete("purse/{purseId}")]
        public async Task<IActionResult> DeletePurse(Guid purseId)
        {
            await _purseRepository.RemovePurse(purseId);
            return Ok();
        }
    }
}