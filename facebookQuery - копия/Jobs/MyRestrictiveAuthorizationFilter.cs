using Hangfire.Dashboard;

namespace Jobs
{
    public class MyRestrictiveAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }
}
