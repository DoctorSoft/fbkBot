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
            
            homeService.GetUnreadMessages(currentAccount.UserId);

            return View(accounts);
        }
    }
}