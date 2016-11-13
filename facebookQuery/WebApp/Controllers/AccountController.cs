using System.Web.Mvc;
using Services.Services;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly HomeService homeService;
        private readonly FacebookMessagesService facebookMessagesService;

        public AccountController()
        {
            this.homeService = new HomeService();
            this.facebookMessagesService = new FacebookMessagesService();
        }
        // GET: Account
        public ActionResult Index(long accountId)
        {
            var currentAccount = homeService.GetAccountByUserId(accountId);

            var status = homeService.GetNewNotices(currentAccount.UserId);

            currentAccount.NumberNewFriends = status.NumberNewFriends;
            currentAccount.NumberNewMessages = status.NumberNewMessages;
            currentAccount.NumberNewNotifications = status.NumberNewNotifications;

            currentAccount.NewMessagesList = facebookMessagesService.GetUnreadMessages_Temp(currentAccount.UserId);
            
            return View(currentAccount);
        }
    }
}