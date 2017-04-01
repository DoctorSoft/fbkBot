using System.Web.Mvc;
using Jobs.JobsService;
using Services.Services;

namespace WebApp.Controllers
{
    public class DeletedUsersController : Controller
    {
        private readonly HomeService _homeService;

        public DeletedUsersController()
        {
            this._homeService = new HomeService(new JobService(), new BackgroundJobService());
        }

        // GET: Users
        public ActionResult Index()
        {
            var accounts = _homeService.GetDeletedAccounts();
            return View(accounts);
        }

        public ActionResult RecoverAccount(long accountId)
        {
            var backgroundJobService = new BackgroundJobService();

            _homeService.RecoverAccount(accountId, backgroundJobService);
            return RedirectToAction("Index", "DeletedUsers");
        } 
    }
}