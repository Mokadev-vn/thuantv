using AutoMapper;
using BELibrary;
using BELibrary.Core.Entity;
using BELibrary.DbContext;
using BELibrary.Entity;
using BELibrary.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CinemaOnline.Areas.Admin.Controllers
{
    public class SeatTypeController : BaseController
    {
        private const string KeyElement = "Loại ghế";

        // GET: Admin/FilmMovieType
        public ActionResult Index()
        {
            ViewBag.Feature = "Danh sách";
            ViewBag.Element = KeyElement;
            if (Request.Url != null) ViewBag.BaseURL = Request.Url.LocalPath;

            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var listData = workScope.SeatTypes.GetAll().ToList();
                return View(listData);
            }
        }

        [HttpPost]
        public JsonResult GetJson(Guid? id)
        {
            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var seatType = workScope.SeatTypes.FirstOrDefault(x => x.Id == id);
                return seatType == default ?
                    Json(new
                    {
                        status = false,
                        mess = "Có lỗi xảy ra: " + ErrorCodes.NotFound.News
                    }) :
                    Json(new
                    {
                        status = true,
                        mess = "Lấy thành công " + KeyElement,
                        data = Mapper.Map<SeatType, SeatTypeDto>(seatType)
                    });
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult CreateOrEdit(SeatTypeDto input, bool isEdit)
        {
            try
            {
                if (isEdit)
                {
                    using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
                    {
                        workScope.SeatTypes.Update(input);
                        workScope.Seats.GetAll().Where(s => s.SeatTypeId == input.Id).ToList().ForEach(s => s.Price = input.Price);
                        workScope.Seats.GetAll().Where(s => s.SeatTypeId == input.Id).ToList().ForEach(s => s.Color = input.Color);
                        workScope.Complete();
                        return Json(new { status = true, mess = "Sửa thành công " + KeyElement });
                    }
                }
                else
                {
                    input.IsDelete = false;
                    input.Id = Guid.NewGuid();
                    using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
                    {
                        workScope.SeatTypes.Insert(input);
                        workScope.Complete();
                        return Json(new { status = true, mess = "Thêm thành công " + KeyElement });
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
                    var userId = GetCurrentUser().Id;
                    workScope.SeatTypes.Del(id, userId);
                    var seats = workScope.Seats.GetAll().Where(s => s.SeatTypeId == id);
                    foreach (var seat in seats)
                    {
                        workScope.Seats.Del(seat.Id, userId);
                    }
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