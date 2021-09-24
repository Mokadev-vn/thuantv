using System.Web.Mvc;

namespace CinemaOnline.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        // GET: Admin/Dashboard
        public ActionResult Index(string mess)
        {
            ViewBag.Element = "Hệ thống";
            ViewBag.Feature = "Bảng điều khiển";
            if (Request.Url != null) ViewBag.BaseURL = Request.Url.LocalPath;

            ViewBag.Message = mess;
            return View();
        }
    }
}