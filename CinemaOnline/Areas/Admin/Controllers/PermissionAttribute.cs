using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CinemaOnline.Areas.Admin.Controllers
{
    public class CinemaOnline : AuthorizeAttribute
    {
        /// <summary>
        /// 1. Admin
        /// 2. Employee
        /// 3. Customer
        /// </summary>
        public int Role { set; get; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var tempData = filterContext.Controller.TempData;
            tempData["Messages"] = "Bạn không có quyền truy cập mục này";
            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "action", "E401" },
                    { "controller", "Error" },
                    { "Area", "" }
                });
        }
    }
}