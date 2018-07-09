using System.Web.Mvc;
using Microsoft.Owin.Host.SystemWeb;

namespace Project2.Controllers
{
    [Authorize(Roles = "User, Admin, God")]
    //[Authorize]
    public class HomeController : Controller
    {
        //[Authorize(Roles = "User, Admin")]
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Chat()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult MeettheTeam()
        {
            return View();

        }
    }

}