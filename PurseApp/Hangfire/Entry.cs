using System;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace PurseApp.Hangfire
{
    public static class Entry
    {
      
        public static IServiceCollection AddHangfireService(this IServiceCollection serviceCollection, string hangfireDbConnectionString)
        {
            serviceCollection.AddHangfire(x => x.UseSqlServerStorage(hangfireDbConnectionString));
            serviceCollection.AddTransient<TransactionWorker>();
            return serviceCollection;
        }

        public static IApplicationBuilder UseHangfire(this IApplicationBuilder app)
        {
            var dashBoardAddress = "/HangfireDashboard";
            app.UseHangfireDashboard(dashBoardAddress, new DashboardOptions());
            app.UseHangfireServer(new BackgroundJobServerOptions());
            app.AddJobs();
            return app;
        }

        private static void AddJobs(this IApplicationBuilder app)
        {
            RecurringJob.AddOrUpdate<TransactionWorker>("transactionWorkedId", (x) => x.Run(), "*/5 * * * *", TimeZoneInfo.Local);
        }
    }
}