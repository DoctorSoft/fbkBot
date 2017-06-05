using System.Web.Configuration;
using Jobs;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Builder;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebApp.Startup))]
namespace WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            HangfireBootstrapper.RegistreJobs(app);

            var hubConfiguration = new HubConfiguration
            {
                EnableDetailedErrors = true, 
                EnableJavaScriptProxies = true
            };

            app.MapSignalR("/signalr", hubConfiguration);
        }
    }
}
