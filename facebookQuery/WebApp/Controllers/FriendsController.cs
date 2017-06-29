using System.Web.Mvc;
using Constants.MessageEnums;
using Services.Services;
using Services.ViewModels.FriendsModels;
using Services.ViewModels.PageModels;

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
        public ActionResult Index(long accountId, MessageRegime? regime, long? all, int pageNumber)
        {
            FriendListViewModel friends;
            ViewBag.Regime = regime;
            ViewBag.AllFriends = null;

            var pageSize = 100;

            if (all != null)
            {
                ViewBag.AllFriends = 1;
                friends = _friendsService.GetFriendsByAccountId(accountId, pageNumber, pageSize);
                return View(friends);
            }
            if (regime == MessageRegime.NoMessages)
            {
                friends = _friendsService.GetFriendsByMessageRegime(accountId, null, pageNumber, pageSize);
                return View(friends);
            }

            friends = _friendsService.GetFriendsByMessageRegime(accountId, regime, pageNumber, pageSize);
            ViewBag.Regime = regime;

            return View(friends);
        }

        public ActionResult RemoveFriend(long accountId, long friendId)
        {
            _friendsService.RemoveFriend(accountId, friendId);
            return RedirectToAction("Index", "Friends", new { accountId });
        }
    }
}