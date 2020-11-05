using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PurseApp.Models;

namespace PurseApp.Repositories
{
    public class PurseRepository : IPurseRepository
    {
        private readonly PurseAppDbContext _dbContext;
        
        public PurseRepository(PurseAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<Purse> CreatePurse(Guid userId)
        {
            var purse = new Purse{UserId = userId};
            await _dbContext.Purses.AddAsync(purse);
            await _dbContext.SaveChangesAsync();
            return purse;
        }

        public async Task<Purse> GetPurse(Guid purseId)
        {
            return await _dbContext.Purses.Include(s => s.Accounts).ThenInclude(s => s.Currency)
                .FirstOrDefaultAsync(s => s.PurseId == purseId);
        }

        public async Task<IEnumerable<Purse>> GetPurses(Guid userId)
        {
            return await _dbContext.Purses.Where(s => s.UserId == userId).Include(s => s.Accounts).ThenInclude(s => s.Currency).ToListAsync();
        }

        public async Task<bool> IsPurseExists(Guid purseId)
        {
            return await _dbContext.Purses.AnyAsync(s => s.PurseId == purseId);
        }
        
        public async Task RemovePurse(Guid purseId)
        {
            var purse = await _dbContext.Purses.FirstOrDefaultAsync(s => s.PurseId == purseId);
            if (purse != null)
            {
                _dbContext.Purses.Remove(purse);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}