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
    public class GalleryController : BaseController
    {
        private readonly string KeyElement = "Thư viện";

        // GET: Admin/Gallery
        public ActionResult Index()
        {
            ViewBag.Feature = "Danh sách";
            ViewBag.Element = KeyElement;

            if (Request.Url != null) ViewBag.BaseURL = Request.Url.LocalPath;

            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var listData = workScope.Galleries.GetAll().ToList();
                return View(listData);
            }
        }

        public ActionResult Create()
        {
            ViewBag.Feature = "Thêm mới";
            ViewBag.Element = KeyElement;
            if (Request.Url != null)
                ViewBag.BaseURL = string.Join("", Request.Url.Segments.Take(Request.Url.Segments.Length - 1));

            ViewBag.IsEdit = false;
            return View();
        }

        public ActionResult Update(Guid id)
        {
            ViewBag.isEdit = true;
            ViewBag.Feature = "Cập nhật";
            ViewBag.Element = KeyElement;
            if (Request.Url != null)
            {
                ViewBag.BaseURL = Request.Url.LocalPath;

                ViewBag.BaseURL = string.Join("", Request.Url.Segments.Take(Request.Url.Segments.Length - 1));
            }

            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var gallery = workScope.Galleries
                    .FirstOrDefault(x => x.Id == id && !x.IsDelete);

                if (gallery != null)
                {
                    return View("Create", gallery);
                }
                else
                {
                    return RedirectToAction("Create", "Gallery");
                }
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult CreateOrEdit(Gallery input, bool isEdit)
        {
            try
            {
                if (isEdit) //update
                {
                    using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
                    {
                        var elm = workScope.Galleries.Get(input.Id);

                        if (elm != null) //update
                        {
                            //Che bien du lieu

                            input.CreationTime = DateTime.Now;

                            elm = input;
                            elm.IsDelete = false;

                            workScope.Galleries.Put(elm, elm.Id);
                            workScope.Complete();

                            return Json(new { status = true, mess = "Cập nhập thành công " });
                        }
                        else
                        {
                            return Json(new { status = false, mess = "Không tồn tại " + KeyElement });
                        }
                    }
                }
                else //Thêm mới
                {
                    using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
                    {
                        //Che bien du lieu
                        input.Id = Guid.NewGuid();
                        input.IsDelete = false;
                        input.CreationTime = DateTime.Now;

                        workScope.Galleries.Add(input);
                        workScope.Complete();
                    }
                    return Json(new { status = true, mess = "Thêm thành công " + KeyElement });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = false,
                    mess = "Có lỗi xảy ra: " + ex.Message
                });
            }
        }

        [HttpPost]
        public JsonResult Del(Guid id)
        {
            try
            {
                using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
                {
                    var elm = workScope.Galleries.Get(id);
                    if (elm != null)
                    {
                        //del
                        workScope.Galleries.Remove(elm);
                        workScope.Complete();
                        return Json(new { status = true, mess = "Xóa thành công " + KeyElement });
                    }
                    else
                    {
                        return Json(new { status = false, mess = "Không tồn tại " + KeyElement });
                    }
                }
            }
            catch
            {
                return Json(new { status = false, mess = "Thất bại" });
            }
        }
    }
}