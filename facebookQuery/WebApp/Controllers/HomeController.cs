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
            homeService.RefreshCookies(100013726390504, "ms.nastasia.1983@mail.ru", "Ntvyjnf123");
            //homeService.SendMessage(100013726390504, 5233296, "Hello");
            var accounts = homeService.GetAccounts();
            return View(accounts);
        }
    }
}