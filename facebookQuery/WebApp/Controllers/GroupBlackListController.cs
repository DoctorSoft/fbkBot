using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Owin.Security.Provider;
using Services.Services;

namespace WebApp.Controllers
{
    public class GroupBlackListController : Controller
    {       
        private readonly FriendsBlackListServices friendsBlackListServices;

        public GroupBlackListController()
        {
            this.friendsBlackListServices = new FriendsBlackListServices();
        }
        // GET: GroupSettings
        public ActionResult Index(long? groupId)
        {
            if (groupId == null)
            {
                return View();
            }

            var groups = friendsBlackListServices.GetFriendsBlackListByGroupId((long)groupId);
            return View(groups);
        }

        public ActionResult ClearBlackList(long groupId)
        {
            friendsBlackListServices.ClearBlackList(groupId);
            return RedirectToAction("Index", "GroupBlackList", new { groupId });
        }
    }
}