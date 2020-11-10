using System.Threading.Tasks;

namespace PurseApp
{
    public interface IWorker
    {
        Task Run();
    }
}