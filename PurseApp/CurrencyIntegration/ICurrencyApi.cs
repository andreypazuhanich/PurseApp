using System.Threading.Tasks;

namespace PurseApp.CurrencyIntegration
{
    public interface ICurrencyApi
    {
        Task<ValCurs> GetApi();
    }
}