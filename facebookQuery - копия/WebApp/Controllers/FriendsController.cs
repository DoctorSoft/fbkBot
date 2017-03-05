using System.Web.Mvc;
using System.Web.Routing;
using Services.Services;

namespace WebApp.Controllers
{
    public class FriendsController : Controller
    {
        private readonly FriendsService _friendsService;

        public FriendsController()
        {
            this._friendsService = new FriendsService(null);
        }

        // GET: Friends
        public ActionResult Index(long accountId)
        {
            var friends = _friendsService.GetFriendsByAccount(accountId);
            return View(friends);
        }

        public ActionResult RemoveFriend(long accountId, long friendId)
        {
            _friendsService.RemoveFriend(accountId, friendId);
            return RedirectToAction("Index", "Friends", new { accountId });
        }
    }
}