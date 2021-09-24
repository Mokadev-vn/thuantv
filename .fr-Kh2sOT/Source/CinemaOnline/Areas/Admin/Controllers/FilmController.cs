using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BELibrary.Core.Entity;
using BELibrary.Core.Utils;
using BELibrary.DbContext;
using BELibrary.Entity;

namespace CinemaOnline.Areas.Admin.Controllers
{
    public class FilmController : BaseController
    {
        // GET: Admin/Film
        private readonly string _keyElement = "Phim";

        public ActionResult Index()
        {
            ViewBag.Feature = "Danh sách";
            ViewBag.Element = _keyElement;
            if (Request.Url != null) ViewBag.BaseURL = Request.Url.LocalPath;

            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var listData = workScope.Films.GetAll().ToList();
                return View(listData);
            }
        }

        public ActionResult Create()
        {
            ViewBag.Feature = "Thêm mới";
            ViewBag.Element = _keyElement;
            if (Request.Url != null)
                ViewBag.BaseURL = string.Join("", Request.Url.Segments.Take(Request.Url.Segments.Length - 1));

            ViewBag.Countries = new SelectList(CountryKey.GetAll(), "Value", "Text");

            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var movieTypes = workScope.MovieTypes.GetAll().ToList();
                ViewBag.MovieTypes = new SelectList(movieTypes, "Id", "Name");

                var movieDisplayTypes = workScope.MovieDisplayTypes.GetAll().ToList();
                ViewBag.MovieDisplayTypes = new SelectList(movieDisplayTypes, "Id", "Name");

                ViewBag.SelectedMovieTypes = new List<string>();
                ViewBag.SelectedFilmMovieDisplayTypes = new List<string>();
            }

            ViewBag.isEdit = false;
            return View();
        }

        public ActionResult Update(Guid id)
        {
            ViewBag.isEdit = true;
            ViewBag.Feature = "Cập nhật";
            ViewBag.Element = _keyElement;
            if (Request.Url != null)
            {
                ViewBag.BaseURL = Request.Url.LocalPath;

                ViewBag.BaseURL = string.Join("", Request.Url.Segments.Take(Request.Url.Segments.Length - 1));
            }

            ViewBag.Countries = new SelectList(CountryKey.GetAll(), "Value", "Text");

            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var movieTypes = workScope.MovieTypes.GetAll().ToList();
                ViewBag.MovieTypes = new SelectList(movieTypes, "Id", "Name");

                var movieDisplayTypes = workScope.MovieDisplayTypes.GetAll().ToList();
                ViewBag.MovieDisplayTypes = new SelectList(movieDisplayTypes, "Id", "Name");

                var film = workScope.Films
                    .Include(x => x.FilmMovieTypes)
                    .FirstOrDefault(x => x.Id == id && !x.IsDelete);

                if (film != null)
                {
                    ViewBag.SelectedMovieTypes = film.FilmMovieTypes.Select(x => x.MovieTypeId.ToString()).ToList();

                    ViewBag.SelectedFilmMovieDisplayTypes = workScope.FilmMovieDisplayTypes
                        .Query(x => x.FilmId == film.Id)
                        .Select(x => x.MovieDisplayTypeId.ToString()).ToList();

                    return View("Create", film);
                }
                else
                {
                    return RedirectToAction("Create", "Film");
                }
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult CreateOrEdit(Film input, bool isEdit, List<Guid> movieTypes, List<Guid> movieDisplayTypes)
        {
            try
            {
                if (isEdit) //update
                {
                    using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
                    {
                        var elm = workScope.Films.Get(input.Id);
                        if (elm != null) //update
                        {
                            elm = input;
                            elm.IsDelete = false;

                            workScope.Films.Put(elm, elm.Id);
                            workScope.Complete();

                            // xử lý FilmDisplayType (Loại film)

                            var guidsFilmMovieDisplayTypesCurrent = workScope.FilmMovieDisplayTypes
                                .Query(x => x.FilmId == elm.Id).Select(x => x.MovieDisplayTypeId).ToList();

                            //bị xóa
                            var delFilmDtypes = guidsFilmMovieDisplayTypesCurrent.Except(movieDisplayTypes);
                            foreach (var item in delFilmDtypes)
                            {
                                var movieType = workScope.FilmMovieDisplayTypes.FirstOrDefault(x => x.MovieDisplayTypeId == item && x.FilmId == elm.Id);
                                workScope.FilmMovieDisplayTypes.Remove(movieType);
                            }
                            workScope.Complete();

                            //Được thêm
                            var addFilmDtypes = movieDisplayTypes.Except(guidsFilmMovieDisplayTypesCurrent);
                            foreach (var item in addFilmDtypes)
                            {
                                workScope.FilmMovieDisplayTypes.Add(new FilmMovieDisplayType
                                {
                                    FilmId = elm.Id,
                                    MovieDisplayTypeId = item
                                });
                            }
                            workScope.Complete();

                            // xử lý Film movie type
                            var filmMovieTypesCurrent = workScope.FilmMovieTypes.Query(x => x.FilmId == elm.Id).Select(x => x.MovieTypeId).ToList();

                            //bị xóa
                            var delFilmMVtypes = filmMovieTypesCurrent.Except(movieTypes);
                            foreach (var item in delFilmMVtypes)
                            {
                                var movieType = workScope.FilmMovieTypes.FirstOrDefault(x => x.MovieTypeId == item && x.FilmId == elm.Id);
                                workScope.FilmMovieTypes.Remove(movieType);
                            }
                            workScope.Complete();

                            //Được thêm
                            var addFilmMVtypes = movieTypes.Except(filmMovieTypesCurrent);
                            foreach (var item in addFilmMVtypes)
                            {
                                workScope.FilmMovieTypes.Add(new FilmMovieType
                                {
                                    FilmId = elm.Id,
                                    MovieTypeId = item
                                });
                            }
                            workScope.Complete();
                            return Json(new { status = true, mess = "Cập nhập thành công " });
                        }
                        else
                        {
                            return Json(new { status = false, mess = "Không tồn tại " + _keyElement });
                        }
                    }
                }
                else
                {
                    using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
                    {
                        input.Id = Guid.NewGuid();
                        input.IsDelete = false;

                        workScope.Films.Add(input);
                        workScope.Complete();

                        //Thêm MovieDisplayTypes
                        foreach (var item in movieDisplayTypes)
                        {
                            workScope.FilmMovieDisplayTypes.Add(
                               new FilmMovieDisplayType
                               {
                                   FilmId = input.Id,
                                   MovieDisplayTypeId = item
                               });
                        }

                        foreach (var item in movieTypes)
                        {
                            workScope.FilmMovieTypes.Add(
                               new FilmMovieType
                               {
                                   FilmId = input.Id,
                                   MovieTypeId = item
                               });
                        }

                        workScope.Complete();
                    }
                    return Json(new { status = true, mess = "Thêm thành công " + _keyElement });
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
                    var elm = workScope.Films.Get(id);
                    if (elm != null)
                    {
                        //Del movie type Film
                        var filmMovieTypes = workScope.FilmMovieTypes
                             .Query(x => x.FilmId == elm.Id);

                        foreach (var item in filmMovieTypes)
                        {
                            workScope.FilmMovieTypes.Remove(item);
                        }

                        //Del movie display type
                        var filmMovieDisplayTypes = workScope.FilmMovieDisplayTypes
                            .Query(x => x.FilmId == elm.Id);

                        foreach (var item in filmMovieDisplayTypes)
                        {
                            workScope.FilmMovieDisplayTypes.Remove(item);
                        }

                        //del film
                        elm.IsDelete = true;
                        elm.DeletetionTime = DateTime.Now;
                        elm.DeleterId = GetCurrentUser().Id;
                        workScope.Films.Put(elm, elm.Id);
                        workScope.Complete();
                        return Json(new { status = true, mess = "Xóa thành công " + _keyElement });
                    }
                    else
                    {
                        return Json(new { status = false, mess = "Không tồn tại " + _keyElement });
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