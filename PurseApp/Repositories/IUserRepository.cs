using System;
using System.Threading.Tasks;
using PurseApp.Models;

namespace PurseApp.Repositories
{
    public interface IUserRepository
    {
        Task<User> CreateUser(string name,string inn);
        Task<User> GetUserById(Guid userId);
        Task<bool> IsUserExists(Guid userId);
    }
}