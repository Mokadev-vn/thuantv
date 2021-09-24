using BELibrary.Core.Entity;
using BELibrary.DbContext;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CinemaOnline.Controllers
{
    public class GalleryController : Controller
    {
        // GET: Gallery
        public ActionResult Index(int? page)
        {
            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var listData = workScope.Galleries.GetAll().ToList();

                var pageNumber = (page ?? 1);
                const int pageSize = 9;
                return View(listData.ToPagedList(pageNumber, pageSize));
            }
        }
    }
}