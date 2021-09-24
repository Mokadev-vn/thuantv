using AutoMapper;
using BELibrary.Core.Entity;
using BELibrary.DbContext;
using BELibrary.Models.View;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CinemaOnline.Controllers
{
    public class FilmController : Controller
    {
        // GET: Film
        public ActionResult Index(int? page)
        {
            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var films = workScope.Films.GetAll();

                //TODAY BEST CHOICE

                var filmViews = Mapper.Map<List<FilmView>>(films);

                foreach (var filmView in filmViews)
                {
                    filmView.FilmMovieTypes = workScope.FilmMovieTypes.GetListFilmMovieType(filmView.Id);
                    filmView.FilmMovieDisplayTypes = workScope.FilmMovieDisplayTypes.GetListFilmMovieDisplayType(filmView.Id);
                    filmView.Name = filmView.Name + " (" + string.Join(", ", filmView.FilmMovieDisplayTypes.ToArray()) + ")";
                }
                filmViews.AddRange(filmViews);
                filmViews.AddRange(filmViews);

                //NOW IN THE CINEMA
                var pageNumber = (page ?? 1);
                const int pageSize = 6;
                return View(filmViews.ToPagedList(pageNumber, pageSize));
            }
        }

        public ActionResult Detail(Guid? id)
        {
            if (!id.HasValue)
            {
                return Redirect("/home/e404");
            }
            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var baseNow = DateTime.Now.AddHours(2);
                var film = workScope.Films.IncludeFilter(f => f.MovieCalendars.Where(mc => mc.IsDelete == false && mc.DateTimeDetail.CompareTo(baseNow) == 1).OrderBy(mc => mc.DateTimeDetail)).FirstOrDefault(x => x.Id == id && !x.IsDelete);
                if (film != null)
                {
                    foreach (var mc in film.MovieCalendars)
                    {
                        mc.DaysOfWeek = workScope.DaysOfWeeks.FirstOrDefault(dow => dow.Id == mc.DaysOfWeekId);
                        mc.TimeFrame = workScope.TimeFrames.FirstOrDefault(tf => tf.Id == mc.TimeFrameId);
                        mc.MovieDisplayType = workScope.MovieDisplayTypes.FirstOrDefault(mdt => mdt.Id == mc.MovieDisplayTypeId);
                        mc.CinemaRoom = workScope.CinemaRooms.FirstOrDefault(cr => cr.Id == mc.CinemaRoomId);                        
                    }
                    var filmView = Mapper.Map<FilmView>(film);
                    filmView.FilmMovieTypes = workScope.FilmMovieTypes.GetListFilmMovieType(film.Id);
                    return View(filmView);
                }
                else
                {
                    return Redirect("/home/e404");
                }
                //TODAY BEST CHOICE
            }
        }

        public ActionResult Search(string filmName, string actor, string director, string country, string keyword, string category, int? page)
        {
            if (filmName == "")
            {
                filmName = null;
            }
            if (actor == "")
            {
                actor = null;
            }
            if (director == "")
            {
                director = null;
            }
            if (country == "")
            {
                country = null;
            }
            if (keyword == "")
            {
                keyword = null;
            }
            var pageNumber = (page ?? 1);
            const int pageSize = 6;
            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var listData = workScope.Films.Query(x => !x.IsDelete).OrderByDescending(x => x.ReleaseDate).ToList();
                if (!string.IsNullOrEmpty(category))
                {
                    var filmViews = Mapper.Map<List<FilmView>>(listData);

                    foreach (var filmView in filmViews)
                    {
                        filmView.FilmMovieTypes = workScope.FilmMovieTypes.GetListFilmMovieType(filmView.Id);
                        filmView.FilmMovieDisplayTypes = workScope.FilmMovieDisplayTypes.GetListFilmMovieDisplayType(filmView.Id);
                    }
                    var result = filmViews.Where(x =>
                        !string.IsNullOrEmpty(string.Join(",", x.FilmMovieTypes))
                        && string.Join(",", x.FilmMovieTypes).ToLower().Contains(category.ToLower())).ToList();

                    return View(result.ToPagedList(pageNumber, pageSize));
                }

                if (keyword == null)
                {
                    var q = from mt in listData
                            where (!string.IsNullOrEmpty(filmName) && mt.Name.ToLower().Contains(filmName.ToLower()))
                                  || (!string.IsNullOrEmpty(actor) && mt.Actors.ToLower().Contains(actor.ToLower()))
                                  || (!string.IsNullOrEmpty(country) && mt.Country.ToLower().Contains(country.ToLower()))
                                  || (!string.IsNullOrEmpty(director) && mt.Directors.ToLower().Contains(director.ToLower()))
                            select mt;

                    var filmViews = Mapper.Map<List<FilmView>>(q);

                    foreach (var filmView in filmViews)
                    {
                        filmView.FilmMovieTypes = workScope.FilmMovieTypes.GetListFilmMovieType(filmView.Id);
                        filmView.FilmMovieDisplayTypes = workScope.FilmMovieDisplayTypes.GetListFilmMovieDisplayType(filmView.Id);
                    }
                    return View(filmViews.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    var q = from mt in listData
                            where (!string.IsNullOrEmpty(keyword) && mt.Name.ToLower().Contains(keyword.ToLower()))
                                  || (!string.IsNullOrEmpty(keyword) && mt.Actors.ToLower().Contains(keyword.ToLower()))
                                  || (!string.IsNullOrEmpty(keyword) && mt.Country.ToLower().Contains(keyword.ToLower()))
                                  || (!string.IsNullOrEmpty(keyword) && mt.Directors.ToLower().Contains(keyword.ToLower()))
                            select mt;

                    var filmViews = Mapper.Map<List<FilmView>>(q);
                    foreach (var filmView in filmViews)
                    {
                        filmView.FilmMovieTypes = workScope.FilmMovieTypes.GetListFilmMovieType(filmView.Id);
                        filmView.FilmMovieDisplayTypes = workScope.FilmMovieDisplayTypes.GetListFilmMovieDisplayType(filmView.Id);
                    }
                    return View(filmViews.ToPagedList(pageNumber, pageSize));
                }
            }
        }
    }
}