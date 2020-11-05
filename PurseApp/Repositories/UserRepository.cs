using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PurseApp.Models;

namespace PurseApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PurseAppDbContext _dbContext;

        public UserRepository(PurseAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<User> GetUserById(Guid userId)
        {
            if (await IsUserExists(userId))
                return await _dbContext.Users.FirstOrDefaultAsync(s => s.UserId == userId);
            return null;
        }

        public async Task<bool> IsUserExists(Guid userId)
        {
            return await _dbContext.Users.AnyAsync(s => s.UserId == userId);
        }

        public async Task<User> CreateUser(string name,string inn)
        {
            if (!await _dbContext.Users.AnyAsync(s => s.INN == inn))
            {
                var user= new User{Name = name,INN = inn}; 
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
                return user;
            }
            return null;
        }
    }
}