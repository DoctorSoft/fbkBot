using System.Web.Mvc;
using Services.Services;

namespace WebApp.Controllers
{
    public class FriendsOutgoingRequestsController : Controller
    {
        private readonly FriendsService _friendsService;

        public FriendsOutgoingRequestsController()
        {
            this._friendsService = new FriendsService(null);
        }

        // GET: Friends
        public ActionResult Index(long accountId)
        {
            var friends = _friendsService.GetOutgoingRequestsFriendshipByAccount(accountId);
            return View(friends);
        }
    }
}