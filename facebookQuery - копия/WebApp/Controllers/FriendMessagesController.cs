using System.Web.Mvc;
using Services.Services;

namespace WebApp.Controllers
{
    public class FriendMessagesController : Controller
    {
        private readonly FriendMessagesService friendMessagesService;

        public FriendMessagesController()
        {
            this.friendMessagesService = new FriendMessagesService();
        }

        // GET: FriendMessages
        public ActionResult Index(long accountId, long friendId)
        {
            var messages = friendMessagesService.GetFriendsMessages(accountId, friendId);
            return View(messages);
        }
    }
}