using System.Web.Mvc;
using Services.Services;

namespace WebApp.Controllers
{
    public class FriendsIncomingRequestsController : Controller
    {
        private readonly FriendsService friendsService;

        public FriendsIncomingRequestsController()
        {
            this.friendsService = new FriendsService();
        }

        // GET: Friends
        public ActionResult Index(long accountId)
        {
            var friends = friendsService.GetIncomingRequestsFriendshipByAccount(accountId);
            return View(friends);
        }

        public ActionResult CancelFriendship(long accountId, long friendFacebookId)
        {
            friendsService.CancelFriendshipRequest(accountId, friendFacebookId);
            return RedirectToAction("Index", "FriendsIncomingRequests", new { accountId });
        }
    }
}