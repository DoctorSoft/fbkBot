using System.Web.Mvc;
using System.Web.Routing;
using Services.Services;

namespace WebApp.Controllers
{
    public class FriendsController : Controller
    {
        private readonly FriendsService friendsService;

        public FriendsController()
        {
            this.friendsService = new FriendsService();
        }

        // GET: Friends
        public ActionResult Index(long accountId)
        {
            var friends = friendsService.GetFriendsByAccount(accountId);
            return View(friends);
        }

        public ActionResult RemoveFriend(long accountId, long friendId)
        {
            friendsService.RemoveFriend(accountId, friendId);
            return RedirectToAction("Index", "Friends", new { accountId });
        }
    }
}