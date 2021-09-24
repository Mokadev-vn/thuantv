using CinemaOnline.Areas.Admin.Authorization;
using System;
using System.Web.Mvc;

namespace CinemaOnline.Controllers
{
    public class BaseController : Controller
    {
        // GET: /Administrator/Base/
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!CookiesManage.ClientLogined())
            {
                var returnUrl = filterContext.HttpContext.Request.RawUrl;
                filterContext.Result =
                      new RedirectResult(string.Concat("~/Account/Login", "?ReturnUrl=", returnUrl));
            }
            base.OnActionExecuting(filterContext);
        }

        public static BELibrary.Entity.User GetCurrentUser()
        {
            return CookiesManage.GetClientUser();
        }
    }
}