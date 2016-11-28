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
            //homeService.RefreshCookies(account.FacebookId, account.Login, account.Password);
            //friendsService.GetFriendsOfFacebook(account.FacebookId);
            return View();
        }
    }
}