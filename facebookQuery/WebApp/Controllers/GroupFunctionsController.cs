using System.Collections.Generic;
using System.Web.Mvc;
using Services.Services;

namespace WebApp.Controllers
{
    public class GroupFunctionsController : Controller
    {
        private readonly GroupFunctionsService groupFunctionsService;

        public GroupFunctionsController()
        {
            this.groupFunctionsService = new GroupFunctionsService();
        }

        // GET: GroupFunctions
        public ActionResult Index()
        {
            var result = groupFunctionsService.GetGroupFunctions();
            return View(result);
        }

        [HttpPost]
        public ActionResult SaveFunctions(long groupId, List<long> functions)
        {
            groupFunctionsService.SaveGroupFunctions(groupId, functions);
            return RedirectToAction("Index", "GroupFunctions");
        }
    }
}