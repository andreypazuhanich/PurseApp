using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PurseApp.AttributesExtension;
using PurseApp.Models;
using PurseApp.Models.Dto;
using PurseApp.Repositories;

namespace PurseApp.Controllers
{
    [Route("api/{userId}/[controller]")]
    [ApiController]
    [AuthorizeExt]
    public class PurseController : ControllerBase
    {
        private readonly IPurseRepository _purseRepository;
        private readonly IMapper _mapper;

        public PurseController(IPurseRepository purseRepository, IMapper mapper)
        {
            _purseRepository = purseRepository;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Purse>>> GetPurses(Guid userId)
        {
            var purses = await _purseRepository.GetPurses(userId);
            if (purses == null)
                return NoContent();
            return Ok(purses);
        }
        
        [HttpGet("accounts")]
        public async Task<ActionResult<IEnumerable<AccountBalance>>> GetPursesBalance(Guid userId)
        {
            var purses = (await _purseRepository.GetPurses(userId)).SelectMany(s => s.Accounts)
                .Select(s => _mapper.Map<AccountBalance>(s));
            return Ok(purses);
        }

        [HttpDelete("{purseId}")]
        public async Task<IActionResult> DeletePurse(Guid userId, Guid purseId)
        {
            await _purseRepository.RemovePurse(purseId);
            return Ok();
        }
    }
}