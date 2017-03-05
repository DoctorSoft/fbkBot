using System.Web.Mvc;
using Services.Services;

namespace WebApp.Controllers
{
    public class FriendsIncomingRequestsController : Controller
    {
        private readonly FriendsService _friendsService;

        public FriendsIncomingRequestsController()
        {
            this._friendsService = new FriendsService(null);
        }

        // GET: Friends
        public ActionResult Index(long accountId)
        {
            var friends = _friendsService.GetIncomingRequestsFriendshipByAccount(accountId);
            return View(friends);
        }

        public ActionResult CancelFriendship(long accountId, long friendFacebookId)
        {
            _friendsService.CancelFriendshipRequest(accountId, friendFacebookId);
            return RedirectToAction("Index", "FriendsIncomingRequests", new { accountId });
        }
    }
}