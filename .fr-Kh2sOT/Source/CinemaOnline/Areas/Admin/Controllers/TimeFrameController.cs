using BELibrary;
using BELibrary.Core.Entity;
using BELibrary.DbContext;
using BELibrary.Entity;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CinemaOnline.Areas.Admin.Controllers
{
    public class TimeFrameController : BaseController
    {
        private const string KeyElement = "Khung giờ";

        // GET: Admin/FilmMovieType
        public ActionResult Index()
        {
            ViewBag.Feature = "Danh sách";
            ViewBag.Element = KeyElement;
            if (Request.Url != null) ViewBag.BaseURL = Request.Url.LocalPath;

            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var listData = workScope.TimeFrames.GetAll().OrderByDescending(x => x.Time).ToList();
                return View(listData);
            }
        }

        [HttpPost]
        public JsonResult GetJson(Guid? id)
        {
            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var timeFrame = workScope.TimeFrames.FirstOrDefault(x => x.Id == id);
                if (timeFrame == default)
                {
                    return Json(new { status = false, mess = "Có lỗi xảy ra: " + ErrorCodes.NotFound.TimeFrame });
                }
                //("HH:mm:ss.SSS")
                var rp = new
                {
                    timeFrame.Id,
                    Time = new DateTime(timeFrame.Time.Ticks).ToString("HH:mm"),
                    timeFrame.Price,
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
        public JsonResult CreateOrEdit(TimeFrame input, bool isEdit)
        {
            try
            {
                if (isEdit)
                {
                    using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
                    {
                        var isExist = workScope.TimeFrames.Query(x => x.Time == input.Time && x.Id == input.Id).Any();
                        if (isExist)
                        {
                            workScope.TimeFrames.Put(input, input.Id);
                            workScope.Complete();
                            return Json(new { status = true, mess = "Sửa thành công " + KeyElement });
                        }
                        else
                        {
                            return Json(new { status = false, mess = $"Đã tồn tại {input.Time}" });
                        }
                    }
                }
                else
                {
                    input.IsDelete = false;
                    input.Id = Guid.NewGuid();
                    using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
                    {
                        var isExist = workScope.TimeFrames.Query(x => x.Time == input.Time).Any();
                        if (!isExist)
                        {
                            workScope.TimeFrames.Add(input);
                            workScope.Complete();
                            return Json(new { status = true, mess = "Thêm thành công " + KeyElement });
                        }
                        else
                        {
                            return Json(new { status = false, mess = $"Đã tồn tại {input.Time}" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, mess = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Del(Guid id)
        {
            try
            {
                using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
                {
                    workScope.TimeFrames.Del(id, GetCurrentUser().Id);
                    return Json(new { status = true, mess = "Xóa thành công " + KeyElement });
                }
            }
            catch
            {
                return Json(new { status = false, mess = "Thất bại" });
            }
        }
    }
}