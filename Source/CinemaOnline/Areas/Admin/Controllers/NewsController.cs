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
    public class NewsController : BaseController
    {
        private const string KeyElement = "Tin tức";

        // GET: Admin/FilmMovieType
        public ActionResult Index()
        {
            ViewBag.Feature = "Danh sách";
            ViewBag.Element = KeyElement;
            if (Request.Url != null) ViewBag.BaseURL = Request.Url.LocalPath;

            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var listData = workScope.News.IncludeFilter(n => n.Creater).Where(n => n.IsDelete == false).ToList();
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

            return View();
        }

        public ActionResult Update(int id)
        {
            ViewBag.isEdit = true;
            ViewBag.key = "Cập nhật";
            ViewBag.Element = KeyElement;
            if (Request.Url != null)
                ViewBag.BaseURL = string.Join("", Request.Url.Segments.Take(Request.Url.Segments.Length - 1));

            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var news = workScope.News.FirstOrDefault(x => x.Id == id);
                if (news == default)
                {
                    throw new Exception(ErrorCodes.NotFound.News);
                }
                return View("Create", news);
            }
        }

        [HttpPost]
        public JsonResult GetJson(int? id)
        {
            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var news = workScope.News.FirstOrDefault(x => x.Id == id);
                return news == default ?
                    Json(new
                    {
                        status = false,
                        mess = "Có lỗi xảy ra: " + ErrorCodes.NotFound.News
                    }) :
                    Json(new
                    {
                        status = true,
                        mess = "Lấy thành công " + KeyElement,
                        data = Mapper.Map<News, NewsDto>(news)
                    });
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult CreateOrEdit(NewsDto input, bool isEdit)
        {
            try
            {
                if (isEdit)
                {
                    using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
                    {
                        workScope.News.Update(input);
                        workScope.Complete();
                        return Json(new { status = true, mess = "Sửa thành công " + KeyElement });
                    }
                }
                else
                {
                    using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
                    {
                        input.CreationTime = DateTime.Now;
                        input.CreaterId = GetCurrentUser().Id;
                        workScope.News.Insert(input);
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
        public JsonResult Del(int id)
        {
            try
            {
                using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
                {
                    workScope.News.Del(id, GetCurrentUser().Id);
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