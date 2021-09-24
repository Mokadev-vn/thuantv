using AutoMapper;
using BELibrary;
using BELibrary.Core.Entity;
using BELibrary.DbContext;
using BELibrary.Entity;
using BELibrary.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebGrease.Css.Extensions;

namespace CinemaOnline.Areas.Admin.Controllers
{
    public class CinemaRoomController : BaseController
    {
        private const string KeyElement = "Phòng chiếu";

        // GET: Admin/FilmMovieType
        public ActionResult Index()
        {
            ViewBag.Feature = "Danh sách";
            ViewBag.Element = KeyElement;
            if (Request.Url != null) ViewBag.BaseURL = Request.Url.LocalPath;

            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var listData = workScope.CinemaRooms.IncludeFilter(cn => cn.Seats.Where(s => s.IsDelete == false))
                    .Where(cr => cr.IsDelete == false).ToList();
                return View(listData);
            }
        }

        public ActionResult Create()
        {
            ViewBag.Feature = "Thêm mới";
            ViewBag.Element = KeyElement;
            ViewBag.isEdit = false;
            if (Request.Url != null)
                ViewBag.BaseURL = string.Join("", Request.Url.Segments.Take(Request.Url.Segments.Length - 1));

            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                ViewBag.SeatTypes = workScope.SeatTypes.GetAll().OrderBy(st => st.Price);
            }

            return View(new CinemaRoom());
        }

        public ActionResult Update(Guid id)
        {
            ViewBag.isEdit = true;
            ViewBag.Feature = "Cập nhật";
            ViewBag.Element = KeyElement;

            if (Request.Url != null)
                ViewBag.BaseURL = string.Join("", Request.Url.Segments.Take(Request.Url.Segments.Length - 1));
            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                ViewBag.SeatTypes = workScope.SeatTypes.GetAll().OrderBy(st => st.Price);
                var cinemaRoom = workScope.CinemaRooms.IncludeFilter(cr => cr.Seats.Where(s => s.IsDelete == false)).FirstOrDefault(x => x.Id == id);
                if (cinemaRoom == default)
                {
                    throw new Exception(ErrorCodes.NotFound.FilmMovieDisplayType);
                }
                cinemaRoom.Seats.ForEach(s =>
                {
                    s.CinemaRoom = null;
                    s.SeatType = null;
                });
                return View("Create", cinemaRoom);
            }
        }

        [HttpPost]
        public JsonResult GetJson(Guid? id)
        {
            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var cinemaRoom = workScope.CinemaRooms.FirstOrDefault(x => x.Id == id);
                return cinemaRoom == default ?
                    Json(new
                    {
                        status = false,
                        mess = "Có lỗi xảy ra: " + ErrorCodes.NotFound.FilmMovieDisplayType
                    }) : Json(new
                    {
                        status = true,
                        mess = "Lấy thành công " + KeyElement,
                        data = Mapper.Map<CinemaRoom, CinemaRoomDto>(cinemaRoom)
                    });
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult CreateOrEdit(CinemaRoomDto input)
        {
            try
            {
                if (input.IsEdit)
                {
                    using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
                    {
                        var seatTypes = workScope.SeatTypes.GetAll().ToList();
                        input.SeatQuantity = input.Seats.Count(s => s.IsDelete == false);
                        input.RowQuantity = input.Seats.Where(s => s.IsDelete == false).GroupBy(s => s.Row).Count();
                        workScope.CinemaRooms.Update(input);
                        foreach (var seat in input.Seats)
                        {
                            seat.CinemaRoomId = input.Id;
                            seat.Price = seatTypes.FirstOrDefault(st => st.Id == seat.SeatTypeId).Price;
                            if (seat.Id == null)
                            {
                                seat.Id = Guid.NewGuid();
                                workScope.Seats.Insert(seat);
                            }
                            else
                            {
                                workScope.Seats.Update(seat);
                            }
                        }
                        workScope.Complete();
                        return Json(new { status = true, mess = "Sửa thành công " + KeyElement });
                    }
                }
                else
                {
                    input.Id = Guid.NewGuid();
                    input.Seats.ForEach(s => s.Id = Guid.NewGuid());
                    input.SeatQuantity = input.Seats.Count();
                    input.RowQuantity = input.Seats.Where(s => s.IsDelete == false).GroupBy(s => s.Row).Count();
                    using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
                    {
                        var seatTypes = workScope.SeatTypes.GetAll();
                        workScope.CinemaRooms.Insert(input);
                        foreach (var seat in input.Seats)
                        {
                            seat.Price = seatTypes.FirstOrDefault(st => st.Id == seat.SeatTypeId).Price;
                            seat.CinemaRoomId = input.Id;
                            workScope.Seats.Insert(seat);
                        }
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
        public JsonResult Del(Guid? id)
        {
            try
            {
                using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
                {
                    workScope.CinemaRooms.Del(id, GetCurrentUser().Id);
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