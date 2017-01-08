using System.Web.Mvc;
using Services.Services;

namespace WebApp.Controllers
{
    public class FriendsOutgoingRequestsController : Controller
    {
        private readonly FriendsService friendsService;

        public FriendsOutgoingRequestsController()
        {
            this.friendsService = new FriendsService();
        }

        // GET: Friends
        public ActionResult Index(long accountId)
        {
            var friends = friendsService.GetOutgoingRequestsFriendshipByAccount(accountId);
            return View(friends);
        }
    }
}