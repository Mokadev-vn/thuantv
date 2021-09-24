using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using BELibrary;
using BELibrary.Core.Entity;
using BELibrary.DbContext;
using BELibrary.Entity;
using BELibrary.Models;

namespace CinemaOnline.Areas.Admin.Controllers
{
    public class MovieDisplayTypeController : BaseController
    {
        private const string keyElement = "Loại trình chiếu";

        // GET: Admin/FilmMovieType
        public ActionResult Index()
        {
            ViewBag.Feature = "Danh sách";
            ViewBag.Element = keyElement;
            if (Request.Url != null) ViewBag.BaseURL = Request.Url.LocalPath;

            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var listData = workScope.MovieDisplayTypes.GetAll().ToList();
                return View(listData);
            }
        }

        [HttpPost]
        public JsonResult GetJson(Guid? id)
        {
            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var movieDisplayType = workScope.MovieDisplayTypes.FirstOrDefault(x => x.Id == id);
                return movieDisplayType == default ?
                    Json(new
                    {
                        status = false,
                        mess = "Có lỗi xảy ra: " + ErrorCodes.NotFound.FilmMovieDisplayType
                    }) :
                    Json(new
                    {
                        status = true,
                        mess = "Lấy thành công " + keyElement,
                        data = Mapper.Map<MovieDisplayType, MovieDisplayTypeDto>(movieDisplayType)
                    });
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult CreateOrEdit(MovieDisplayTypeDto input, bool isEdit)
        {
            try
            {
                if (isEdit)
                {
                    using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
                    {
                        workScope.MovieDisplayTypes.Update(input);
                        workScope.Complete();
                        return Json(new { status = true, mess = "Sửa thành công " + keyElement });
                    }
                }
                else
                {
                    input.Id = Guid.NewGuid();
                    using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
                    {
                        workScope.MovieDisplayTypes.Insert(input);
                        workScope.Complete();
                        return Json(new { status = true, mess = "Thêm thành công " + keyElement });
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
                    workScope.MovieDisplayTypes.Del(id, GetCurrentUser().Id);
                    return Json(new { status = true, mess = "Xóa thành công " + keyElement });
                }
            }
            catch
            {
                return Json(new { status = false, mess = "Thất bại" });
            }
        }
    }
}