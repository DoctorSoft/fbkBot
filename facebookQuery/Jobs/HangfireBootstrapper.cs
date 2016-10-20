using Hangfire;
using Owin;

namespace Jobs
{
    public static class HangfireBootstrapper
    {
        public static void RegistreJobs(IAppBuilder app)
        {
            GlobalConfiguration.Configuration
                .UseSqlServerStorage("DefaultConnection");

            app.UseHangfireDashboard("/dashboard");
            app.UseHangfireServer();
        }
    }
}
