using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class LanguageController : Controller
    {
        // GET: ChangeLanguage
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ChangeCulture(string lang, string returnUrl)
        {
            if (lang != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(lang);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
            }

            var cookie = new HttpCookie("Language")
            {
                Value = lang
            };

            Response.Cookies.Add(cookie);

            return Redirect(returnUrl);
        }
    }
}