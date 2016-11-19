using System.Linq;
using System.Threading;
using System.Web.Mvc;
using Services.Services;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly HomeService homeService;
        private readonly FriendsService friendsService;
        private readonly FacebookMessagesService facebookMessagesService;

        public HomeController()
        {
            this.homeService = new HomeService();
            this.friendsService = new FriendsService();
            this.facebookMessagesService = new FacebookMessagesService();
        }

        public ActionResult Index()
        {
            var accounts = homeService.GetAccounts();
            facebookMessagesService.GetUnreadMessages(accounts.FirstOrDefault().UserId);
            return View(accounts);
        }
    }
}