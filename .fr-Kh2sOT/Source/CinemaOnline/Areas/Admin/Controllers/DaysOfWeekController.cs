using BELibrary;
using BELibrary.Core.Entity;
using BELibrary.DbContext;
using BELibrary.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CinemaOnline.Areas.Admin.Controllers
{
    public class DaysOfWeekController : Controller
    {
        private const string KeyElement = "Ngày chiếu";

        // GET: Admin/DaysOfWeek
        public ActionResult Index()
        {
            ViewBag.Feature = "Danh sách";
            ViewBag.Element = KeyElement;
            if (Request.Url != null) ViewBag.BaseURL = Request.Url.LocalPath;

            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var listData = workScope.DaysOfWeeks.GetAll().OrderBy(x => x.Name).ToList();
                return View(listData);
            }
        }

        [HttpPost]
        public JsonResult GetJson(Guid? id)
        {
            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var daysOfWeek = workScope.DaysOfWeeks.FirstOrDefault(x => x.Id == id);
                if (daysOfWeek == default)
                {
                    return Json(new { status = false, mess = "Có lỗi xảy ra: " + ErrorCodes.NotFound.DaysOfWeek });
                }

                var rp = new
                {
                    daysOfWeek.Id,
                    Name = daysOfWeek.Name,
                    daysOfWeek.Price,
                };

                return Json(new
                {
                    status = true,
                    mess = "Lấy thành công " + KeyElement,
                    data = rp
                });
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult CreateOrEdit(DaysOfWeek input, bool isEdit)
        {
            try
            {
                using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
                {
                    var isExist = workScope.DaysOfWeeks.Query(x => x.Id == input.Id).Any();
                    if (isExist)
                    {
                        workScope.DaysOfWeeks.Put(input, input.Id);
                        workScope.Complete();
                        return Json(new { status = true, mess = "Sửa thành công " + KeyElement });
                    }
                    else
                    {
                        return Json(new { status = false, mess = $"Không tồn tại {input.Name}" });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, mess = "Có lỗi xảy ra: " + ex.Message });
            }
        }
    }
}