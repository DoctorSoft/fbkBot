using System;
using System.Collections.Generic;
using System.Linq;
using Constants.FunctionEnums;
using Hangfire;
using Hangfire.Dashboard;
using Owin;

namespace Jobs
{
    public static class HangfireBootstrapper
    {
        public static void RegistreJobs(IAppBuilder app)
        {
            GlobalConfiguration.Configuration
                .UseSqlServerStorage("DefaultConnection");

            var options = new BackgroundJobServerOptions
            {
                Queues = Enum.GetValues(typeof(FunctionName)).Cast<FunctionName>().Select(name => name.ToString("G").ToLower()).ToArray()
            };

            app.UseHangfireDashboard("/dashboard", new DashboardOptions
            {
                Authorization = new[] { new MyRestrictiveAuthorizationFilter() }
            });

            app.UseHangfireServer(options);

            JobsBootstrapper.SetUpJobs();
        }
    }
}
