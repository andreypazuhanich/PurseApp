using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PurseApp.Models;

namespace PurseApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IPurseRepository _purseRepository;

        public UserRepository(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager, IPurseRepository purseRepository)
        {
            _userManager = userManager;
            _purseRepository = purseRepository;
            _signInManager = signInManager;
        }

        public async Task<bool> Authenticate(User user)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(user.UserName, user.Password,
                user.IsRemember, false);
            return signInResult.Succeeded;
        }

        public async Task Register(User registerRequest)
        {
            var result = await _userManager.CreateAsync(registerRequest, registerRequest.Password);

            if (result.Succeeded)
                await _purseRepository.CreatePurse(Guid.Parse(registerRequest.Id));
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
