using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using PurseApp.Helpers;
using PurseApp.Models.Dto;

namespace PurseApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IPurseRepository _purseRepository;

        public UserRepository(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager, IConfiguration configuration,IPurseRepository purseRepository)
        {
            _userManager = userManager;
            _configuration = configuration;
            _purseRepository = purseRepository;
            _signInManager = signInManager;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest userModel)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(userModel.UserName, userModel.Password,
                userModel.IsRemember, false);
            if (signInResult.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(userModel.UserName);
                var token = _configuration.GenerateJwtToken(user);
                return new AuthenticateResponse{Token = token,UserName = user.UserName};
            }
            return null;
        }

        public async Task<AuthenticateResponse> Register(RegisterRequest registerRequest)
        {
            var user = new IdentityUser {UserName = registerRequest.UserName, Email = registerRequest.Email};
            var result = await _userManager.CreateAsync(user, registerRequest.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                await _purseRepository.CreatePurse(Guid.Parse(user.Id));
                var token = _configuration.GenerateJwtToken(user);
                return new AuthenticateResponse { UserName = user.UserName, Token = token};
            }
            return null;
        }

        public async Task DeleteUsers()
        {
            foreach (var user in _userManager.Users.ToList())
            {
                await _userManager.DeleteAsync(user);
            }
        }
    }
}
