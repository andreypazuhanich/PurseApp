using System;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace PurseApp.Hangfire
{
    public static class Entry
    {
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