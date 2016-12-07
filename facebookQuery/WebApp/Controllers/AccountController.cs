using System.Web.Mvc;
using Jobs.JobsService;
using Services.Services;
using Services.ServiceTools;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly HomeService homeService;
        private readonly FacebookMessagesService facebookMessagesService;

        public AccountController()
        {
            this.homeService = new HomeService(new JobService(), new AccountManager());
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

            currentAccount.NewMessagesList = facebookMessagesService.GetUnreadMessagesFromAccountPage(currentAccount.UserId);
            
            return View(currentAccount);
        }
    }
}