using System.Linq;
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

            //homeService.RefreshCookies(currentAccount.UserId, currentAccount.Login, currentAccount.Password);
            homeService.SendMessage(100013726390504, 100002115472896, "Hello");
            //var status = homeService.GetNewNotices(currentAccount.UserId);

            return View(accounts);
        }
    }
}