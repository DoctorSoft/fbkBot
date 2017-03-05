using System.Collections.Generic;
using System.Web.Mvc;
using Jobs.JobsService;
using Services.Services;

namespace WebApp.Controllers
{
    public class GroupFunctionsController : Controller
    {
        private readonly GroupFunctionsService _groupFunctionsService;

        public GroupFunctionsController()
        {
            this._groupFunctionsService = new GroupFunctionsService();
        }

        // GET: GroupFunctions
        public ActionResult Index()
        {
            var result = _groupFunctionsService.GetGroupFunctions();
            return View(result);
        }

        [HttpPost]
        public ActionResult SaveFunctions(long groupId, List<long> functions)
        {
            _groupFunctionsService.SaveGroupFunctions(groupId, functions, new BackgroundJobService());
            return RedirectToAction("Index", "GroupFunctions");
        }
    }
}