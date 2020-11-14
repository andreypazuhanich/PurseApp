using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PurseApp.Hangfire;

namespace PurseApp.Services
{
    public static class HanfireService
    {
        public static IServiceCollection AddHangfireService(this IServiceCollection serviceCollection, string hangfireDbConnectionString)
        {
            serviceCollection.AddHangfire(x => x.UseSqlServerStorage(hangfireDbConnectionString));
            serviceCollection.AddTransient<TransactionWorker>();
            return serviceCollection;
        }
    }
}