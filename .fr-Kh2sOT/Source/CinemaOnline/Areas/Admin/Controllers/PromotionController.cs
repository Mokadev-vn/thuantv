using AutoMapper;
using BELibrary;
using BELibrary.Core.Entity;
using BELibrary.DbContext;
using BELibrary.Entity;
using BELibrary.Extendsions;
using BELibrary.Models;
using BELibrary.Utils;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CinemaOnline.Areas.Admin.Controllers
{
    public class PromotionController : BaseController
    {
        private const string KeyElement = "Khuyến mãi";

        // GET: Admin/FilmMovieType
        public ActionResult Index()
        {
            ViewBag.Feature = "Danh sách";
            ViewBag.Element = KeyElement;
            if (Request.Url != null) ViewBag.BaseURL = Request.Url.LocalPath;

            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var listData = workScope.Promotions.IncludeFilter(p => p.Film, p => p.Topping).ToList().Where(p => p.IsDelete == false).ToList();
                ViewBag.Films = workScope.Films.GetAll();
                ViewBag.Toppings = workScope.Toppings.GetAll();
                return View(listData);
            }
        }

        [HttpPost]
        public JsonResult GetJson(Guid? id)
        {
            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var promotion = workScope.Promotions.FirstOrDefault(x => x.Id == id);
                return promotion == default ?
                    Json(new
                    {
                        status = false,
                        mess = "Có lỗi xảy ra: " + ErrorCodes.NotFound.Promotion
                    }) :
                    Json(new
                    {
                        status = true,
                        mess = "Lấy thành công " + KeyElement,
                        data = Mapper.Map<Promotion, PromotionDto>(promotion)
                    });
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult CreateOrEdit(PromotionDto input, bool isEdit)
        {
            try
            {
                if (isEdit)
                {
                    using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
                    {
                        input.KindOfPromotionEnum = input.GetKindOfPromotionEnum();
                        input.KindOfPromotion = input.GetKindOfPromotionStr();
                        workScope.Promotions.Update(input);
                        workScope.Complete();
                        return Json(new { status = true, mess = "Sửa thành công " + KeyElement });
                    }
                }
                else
                {
                    input.Id = Guid.NewGuid();
                    using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
                    {
                        input.CreaterId = GetCurrentUser().Id;
                        input.CreationTime = DateTime.Now;
                        input.KindOfPromotionEnum = input.GetKindOfPromotionEnum();
                        input.KindOfPromotion = input.GetKindOfPromotionStr();
                        workScope.Promotions.InsertReturnId(input);
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
                    workScope.Promotions.Del(id, GetCurrentUser().Id);
                    return Json(new { status = true, mess = "Xóa thành công " + KeyElement });
                }
            }
            catch
            {
                return Json(new { status = false, mess = "Thất bại" });
            }
        }

        [HttpPost]
        public JsonResult GetCode()
        {
            var result = "";
            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var promotionCodes = workScope.Promotions.GetAll().Select(p => p.Code).ToList();
                while (string.IsNullOrEmpty(result))
                {
                    result = CodeUtils.RandomString(8);
                    if (promotionCodes.Contains(result))
                    {
                        result = "";
                    }
                }
            }
            return Json(new { status = true, mess = "Tạo thành công code " + result, data = result });
        }
    }
}