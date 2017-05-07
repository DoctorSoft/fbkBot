using Runner.Interfaces;
using Services.Services;

namespace Runner.Runners.Cookies
{
    public class RefreshCookiesRunner : IRunner
    {
        public void Run(IRunnerModel model)
        {
            var account = model.Account;

            var forSpy = model.ForSpy;

            new CookieService().RefreshCookies(account, forSpy);
        }
    }
}
