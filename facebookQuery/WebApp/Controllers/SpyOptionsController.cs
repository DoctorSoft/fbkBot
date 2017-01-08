using System.Web.Mvc;
using Jobs.JobsService;
using Services.Services;

namespace WebApp.Controllers
{
    public class SpyOptionsController : Controller
    {
        private readonly SpyService spyService;

        public SpyOptionsController()
        {
            this.spyService = new SpyService(new JobService());
        }
        // GET: Option
        public ActionResult Index()
        {
            return View();
        }
     }
}