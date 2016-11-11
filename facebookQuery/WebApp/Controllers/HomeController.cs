using System.Linq;
using System.Threading;
using System.Web.Mvc;
using Services.Services;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly HomeService homeService;

        public HomeController()
        {
            this.homeService = new HomeService();
        }

        public ActionResult Index()
        {
            var accounts = homeService.GetAccounts();

            var currentAccount = accounts.FirstOrDefault();

            var s = homeService.GetUnreadMessages(currentAccount.UserId);

            /*foreach (var accountViewModel in accounts)
            {
                homeService.RefreshCookies(accountViewModel.UserId, accountViewModel.Login, accountViewModel.Password);
                Thread.Sleep(5000);
            }*/

            return View(accounts);
        }
    }
}