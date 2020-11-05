using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using PurseApp.Models;

namespace PurseApp.Repositories
{
    public interface IPurseRepository
    {
        Task<Purse> CreatePurse(Guid userId);
        Task<Purse> GetPurse(Guid purseId);
        Task<IEnumerable<Purse>> GetPurses(Guid userId);
        Task<bool> IsPurseExists(Guid purseId);
        Task RemovePurse(Guid purseId);
    }
}