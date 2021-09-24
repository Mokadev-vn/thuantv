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
    public class MovieTypeController : BaseController
    {
        private const string KeyElement = "Loại phim";

        // GET: Admin/FilmMovieType
        public ActionResult Index()
        {
            ViewBag.Feature = "Danh sách";
            ViewBag.Element = KeyElement;

            if (Request.Url != null) ViewBag.BaseURL = Request.Url.LocalPath;

            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var listData = workScope.MovieTypes.GetAll().ToList();
                return View(listData);
            }
        }

        [HttpPost]
        public JsonResult GetJson(Guid? id)
        {
            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var movieType = workScope.MovieTypes.FirstOrDefault(x => x.Id == id);
                return movieType == default ?
                    Json(new
                    {
                        status = false,
                        mess = "Có lỗi xảy ra: " + ErrorCodes.NotFound.FilmMovieType
                    }) :
                    Json(new
                    {
                        status = true,
                        mess = "Lấy thành công " + KeyElement,
                        data = Mapper.Map<MovieType, MovieTypeDto>(movieType)
                    });
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult CreateOrEdit(MovieType input, bool isEdit)
        {
            try
            {
                if (isEdit)
                {
                    using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
                    {
                        workScope.MovieTypes.Put(input, input.Id);
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
                        workScope.MovieTypes.Add(input);
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
                    workScope.MovieTypes.Del(id, GetCurrentUser().Id);
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