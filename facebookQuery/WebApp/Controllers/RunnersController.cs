using System.Web.Mvc;
using Services.Services.Runners;

namespace WebApp.Controllers
{
    public class RunnersController : Controller
    {
        private readonly RunnerService _runnerService;

        public RunnersController()
        {
            this._runnerService = new RunnerService();
        }
        // GET: AdminPage
        public ActionResult Index()
        {
            var runnersList = _runnerService.GetRunners();
            return View(runnersList);
        }

        public ActionResult ChangeRunnerStatus(long runnerId, bool isAllowed)
        {
            _runnerService.UpdateRunner(runnerId, isAllowed);
            return RedirectToAction("Index", "Runners");
        }
    }
}