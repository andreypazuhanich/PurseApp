using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PurseApp.Models;
using PurseApp.Models.Dto;
using PurseApp.Repositories;

namespace PurseApp.Controllers
{
    [Route("api/{userId}/[controller]")]
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
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Purse>>> GetPurses(Guid userId)
        {
            if (! await _userRepository.IsUserExists(userId))
                return NotFound("Пользователь не существует");
            return Ok(await _purseRepository.GetPurses(userId));
        }
        
        [HttpGet("accounts")]
        public async Task<ActionResult<IEnumerable<PurseBalanceDto>>> GetPursesBalance(Guid userId)
        {
            if (! await _userRepository.IsUserExists(userId))
                return NotFound("Пользователь не существует");
            var purses = (await _purseRepository.GetPurses(userId)).SelectMany(s => s.Accounts)
                .Select(s => new PurseBalanceDto {AccountName = s.Name, Balance = s.Balance, Currency = s.Currency.Name});
            return Ok(purses);
        }

        [HttpDelete("{purseId}")]
        public async Task<IActionResult> DeletePurse(Guid userId, Guid purseId)
        {
            if (!await _userRepository.IsUserExists(userId))
                return NotFound();
            await _purseRepository.RemovePurse(purseId);
            return Ok();
        }
    }
}