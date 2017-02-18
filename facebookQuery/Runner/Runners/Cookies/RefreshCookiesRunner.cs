using Runner.Interfaces;
using Services.Services;
using Services.ViewModels.HomeModels;

namespace Runner.Runners.Cookies
{
    public class RefreshCookiesRunner : IRunner
    {
        public void Run(AccountViewModel account)
        {
            new CookieService().RefreshCookies(account);
        }
    }
}
