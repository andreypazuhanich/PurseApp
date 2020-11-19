using System.Threading.Tasks;
using PurseApp.Models;

namespace PurseApp.Repositories
{
    public interface IUserRepository
    {
        Task<bool> Authenticate(User user);
        Task Register(User user);
        Task DeleteUsers();
    }
}