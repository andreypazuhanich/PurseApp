using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserController(IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
        }
        
        
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _userRepository.Register(_mapper.Map<User>(registerRequest));
                }
                catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }
                return Ok();
            }

            return ValidationProblem();
        }
        
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<string>> Login(AuthenticateRequest authenticateRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = _mapper.Map<User>(authenticateRequest);
                    if (!await _userRepository.Authenticate(user))
                        return Unauthorized();
                    var token = _configuration.GenerateJwtToken(user);
                    return Ok($"Bearer {token}"); //TODO: return token для удобства
                }
                catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return ValidationProblem();
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