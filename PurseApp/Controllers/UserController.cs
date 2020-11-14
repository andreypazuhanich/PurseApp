using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PurseApp.Helpers;
using PurseApp.Models;
using PurseApp.Models.Dto;
using PurseApp.Repositories;

namespace PurseApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<AuthenticateResponse>> Register(RegisterRequest registerRequest)
        {
            if (ModelState.IsValid)
            {
                var authenticateResponse = await _userRepository.Register(registerRequest);
                if (authenticateResponse == null)
                    return BadRequest();
                return Ok(authenticateResponse);//TODO: return AuthenticateResponse только для тестов
            }
            return BadRequest();
        }
        
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<AuthenticateResponse>> Login(AuthenticateRequest authenticateRequest)
        {
            if (ModelState.IsValid)
            {
                var response = await _userRepository.Authenticate(authenticateRequest);
                if (response == null)
                    return Unauthorized();
                return Ok(response);//TODO: return AuthenticateResponse только для тестов
            }
            return BadRequest();
        }

        /// <summary>
        ///  для удобства очистки юзеров
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteUsers()
        {
            await _userRepository.DeleteUsers();
            return Ok();
        }
    }
}