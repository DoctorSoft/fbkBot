using System.Web.Mvc;
using Services.Services;

namespace WebApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly HomeService _homeService;

        public UsersController()
        {
            this._homeService = new HomeService();
        }

        // GET: Users
        public ActionResult Index()
        {
            var accounts = _homeService.GetAccounts();
            return View(accounts);
        }

        public ActionResult RemoveAccount(long accountId)
        {
            _homeService.RemoveAccount(accountId);
            return RedirectToAction("Index", "Users");
        } 
    }
}