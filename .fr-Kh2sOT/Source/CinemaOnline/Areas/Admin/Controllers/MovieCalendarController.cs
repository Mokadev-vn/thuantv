using AutoMapper;
using BELibrary.Core.Entity;
using BELibrary.DbContext;
using BELibrary.Entity;
using BELibrary.Models;
using CinemaOnline.Areas.Admin.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BELibrary.Core.Utils;
using BELibrary.Extendsions;
using WebGrease.Css.Extensions;

namespace CinemaOnline.Areas.Admin.Controllers
{
    public class MovieCalendarController : BaseController
    {
        // GET: Admin/MovieCalendar
        private const string KeyElement = "Lịch chiếu phim";

        public ActionResult Index(int? page, Guid? roomId)
        {
            ViewBag.Feature = "Danh sách";
            ViewBag.Element = KeyElement;
            if (Request.Url != null) ViewBag.BaseURL = Request.Url.LocalPath;

            using (var unitOfWork = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var cinemaRooms = unitOfWork.CinemaRooms.GetAll().ToList();

                ViewBag.CinemaRooms = new SelectList(cinemaRooms, "Id", "Name");

                if (!roomId.HasValue)
                {
                    if (cinemaRooms.Count > 0)
                    {
                        roomId = cinemaRooms[0].Id;
                    }
                }
                ViewBag.RoomId = roomId;
                var movieCalendars = unitOfWork.MovieCalendars.Query(x => x.CinemaRoomId == roomId);
                var timeFrames = unitOfWork.TimeFrames.GetAll();
                var films = unitOfWork.Films.GetAll();
                var daysOfWeeks = unitOfWork.DaysOfWeeks.GetAll();
                var movieDisplayTypes = unitOfWork.MovieDisplayTypes.GetAll();

                var listData = (from mvc in movieCalendars
                                join t in timeFrames on mvc.TimeFrameId equals t.Id
                                join f in films on mvc.FilmId equals f.Id
                                join d in daysOfWeeks on mvc.DaysOfWeekId equals d.Id
                                join mdt in movieDisplayTypes on mvc.MovieDisplayTypeId equals mdt.Id
                                select Mapper.Map<MovieCalendarDto>(mvc)).ToList().OrderByDescending(x => x.StartWeek);

                //var test = listData.ToList();

                var newList = listData.GroupBy(x => new { x.FilmId, x.FilmName, x.StartWeek })
                    .Select(y => new MovieCalendarDtoView()
                    {
                        FilmName = y.Key.FilmName,
                        Id = y.Key.FilmId,
                        StartWeek = y.Key.StartWeek,
                        DayOfWeeks = y.GroupBy(tf => new { tf.DaysOfWeekId, tf.DaysOfWeekName }).Select(t => new MovieCalendarDayOfWeek
                        {
                            Day = t.Key.DaysOfWeekName,
                            Type = t.GroupBy(type => new { type.MovieDisplayTypeId, type.MovieDisplayTypeName })
                                    .Select(x => new MovieCalendarMovieType
                                    {
                                        Type = x.Key.MovieDisplayTypeName,
                                        IdObj = new IdentityMovieCalendar
                                        {
                                            FilmId = y.Key.FilmId,
                                            DayOfWeekId = t.Key.DaysOfWeekId,
                                            MovieDisplayTypeId = x.Key.MovieDisplayTypeId,

                                            StartWeek = y.Key.StartWeek
                                        },
                                        Time = t.GroupBy(tr => new { tr.TimeFrameId, tr.TimeFrameName }).Select(time => time.Key.TimeFrameName).ToList()
                                    }).ToList()
                        }).ToList()
                    });
                //var test2 = newList.ToList();
                var pageNumber = (page ?? 1);
                const int pageSize = 4;
                return View(newList.ToPagedList(pageNumber, pageSize));
            }
        }

        public ActionResult Create()
        {
            ViewBag.Feature = "Thêm mới";
            ViewBag.Element = KeyElement;
            if (Request.Url != null)
                ViewBag.BaseURL = string.Join("", Request.Url.Segments.Take(Request.Url.Segments.Length - 1));

            ViewBag.Countries = new SelectList(CountryKey.GetAll(), "Value", "Text");

            using (var unitOfWork = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var films = unitOfWork.Films.GetAll().OrderBy(x => x.Name).ToList();
                ViewBag.Films = new SelectList(films, "Id", "Name");

                var daysOfWeeks = unitOfWork.DaysOfWeeks.GetAll().OrderBy(x => x.Name).ToList();
                ViewBag.DaysOfWeeks = new SelectList(daysOfWeeks, "Id", "Name");

                var timeFrames = unitOfWork.TimeFrames.GetAll().OrderByDescending(x => x.Time).ToList();
                ViewBag.TimeFrames = new SelectList(timeFrames, "Id", "Time");

                var movieDisplayTypes = unitOfWork.MovieDisplayTypes.GetAll().ToList();
                ViewBag.MovieDisplayTypes = new SelectList(movieDisplayTypes, "Id", "Name");

                var cinemaRooms = unitOfWork.CinemaRooms.GetAll().OrderByDescending(x => x.Name).ToList();
                ViewBag.CinemaRooms = new SelectList(cinemaRooms, "Id", "Name");

                ViewBag.SelectedMovieTypes = new List<string>();
                ViewBag.SelectedFilmMovieDisplayTypes = new List<string>();
            }

            ViewBag.IsEdit = false;
            return View();
        }

        [HttpPost]
        public JsonResult GetMovieDisplay(Guid? filmId)
        {
            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var filmMovieDisplayType = workScope.FilmMovieDisplayTypes.Include(x => x.MovieDisplayType).Where(x => x.FilmId == filmId).ToList();

                var displayTypes = filmMovieDisplayType.Select(
                    fdt => fdt.MovieDisplayType).ToList();

                return
                    Json(new
                    {
                        status = true,
                        mess = "Lấy thành công " + KeyElement,
                        data = displayTypes.Select(x => new
                        {
                            x.Id,
                            x.Name
                        })
                    });
            }
        }

        public ActionResult Update(MovieCalendarGet input)
        {
            ViewBag.IsEdit = true;
            ViewBag.Feature = "Cập nhật";
            ViewBag.Element = KeyElement;
            if (Request.Url != null)
            {
                ViewBag.BaseURL = Request.Url.LocalPath;

                ViewBag.BaseURL = string.Join("", Request.Url.Segments.Take(Request.Url.Segments.Length - 1));
            }

            ViewBag.Countries = new SelectList(CountryKey.GetAll(), "Value", "Text");

            try
            {
                using (var unitOfWork = new UnitOfWork(new CinemaOnlineDbContext()))
                {
                    var films = unitOfWork.Films.GetAll().ToList();
                    ViewBag.Films = new SelectList(films, "Id", "Name");

                    var daysOfWeeks = unitOfWork.DaysOfWeeks.GetAll().OrderBy(x => x.Name).ToList();
                    ViewBag.DaysOfWeeks = new SelectList(daysOfWeeks, "Id", "Name");

                    var movieDisplayTypes = unitOfWork.MovieDisplayTypes.GetAll().ToList();
                    ViewBag.MovieDisplayTypes = new SelectList(movieDisplayTypes, "Id", "Name");

                    var timeFrames = unitOfWork.TimeFrames.GetAll().ToList();
                    ViewBag.TimeFrames = new SelectList(timeFrames, "Id", "Time");

                    var cinemaRooms = unitOfWork.CinemaRooms.GetAll().ToList();
                    ViewBag.CinemaRooms = new SelectList(cinemaRooms, "Id", "Name");

                    ViewBag.LastIp = input;

                    var movieCalendar = unitOfWork.MovieCalendars
                        .FirstOrDefault(x => x.FilmId == input.LFilmId
                                             && x.DaysOfWeekId == input.LDayOfWeekId
                                             && x.MovieDisplayTypeId == input.LMovieDisplayTypeId
                                             && x.StartWeek == input.LStartWeek
                                             && !x.IsDelete);

                    var timeFramesSelected = unitOfWork.MovieCalendars
                        .Query(x => x.StartWeek == movieCalendar.StartWeek
                                    && x.FilmId == movieCalendar.FilmId
                                    && x.DaysOfWeekId == movieCalendar.DaysOfWeekId
                                    && x.MovieDisplayTypeId == movieCalendar.MovieDisplayTypeId).Select(x => x.TimeFrameId.ToString()).ToList();

                    ViewBag.SelectedTimeFrames = timeFramesSelected;

                    return View("Create", movieCalendar);
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Dashboard", new { mess = e.Message });
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult CreateOrEdit(MovieCalendar input, bool isEdit, List<Guid> timeFrames, MovieCalendarGet lastIp)
        {
            try
            {
                if (isEdit) //update
                {
                    using (var unitOfWork = new UnitOfWork(new CinemaOnlineDbContext()))
                    {
                        //Idea: Lấy ra những danh sách khung giờ chiếu cũ so sánh với dữ liệu khung giờ mưới
                        //Nếu Có giá trị mới thì thêm vào, không tồn tại giá trị cũ thì xóa đi

                        //Lấy ra danh sách Khung giờ chiếu cũ
                        var listCurrentTimeFrame = unitOfWork.MovieCalendars
                                 .Query(x => x.StartWeek == lastIp.LStartWeek
                                         && x.FilmId == lastIp.LFilmId
                                         && x.DaysOfWeekId == lastIp.LDayOfWeekId
                                         && x.CinemaRoomId == lastIp.LCinemaRoomId
                                         && x.MovieDisplayTypeId == lastIp.LMovieDisplayTypeId)
                                 .Select(x => x.TimeFrameId).ToList();

                        //bị xóa
                        var delMvc = listCurrentTimeFrame.Except(timeFrames);
                        foreach (var delId in delMvc)
                        {
                            var movieCalendarDeled = unitOfWork.MovieCalendars
                               .FirstOrDefault(x => x.StartWeek == lastIp.LStartWeek
                                       && x.FilmId == lastIp.LFilmId
                                       && x.DaysOfWeekId == lastIp.LDayOfWeekId
                                       && x.MovieDisplayTypeId == lastIp.LMovieDisplayTypeId
                                       && x.CinemaRoomId == lastIp.LCinemaRoomId
                                       && x.TimeFrameId == delId);
                            unitOfWork.MovieCalendars.Remove(movieCalendarDeled);
                        }
                        unitOfWork.Complete();

                        //Tính giá của lịch chiếu phim
                        var film = unitOfWork.Films.Get(input.FilmId);
                        var daysOfWeek = unitOfWork.DaysOfWeeks.Get(input.DaysOfWeekId);
                        var movieDisplayType = unitOfWork.MovieDisplayTypes.Get(input.MovieDisplayTypeId);

                        var price = film.Price + daysOfWeek.Price + movieDisplayType.Price;
                        var dayOfWeek = unitOfWork.DaysOfWeeks.FirstOrDefault(dow => dow.Id == input.DaysOfWeekId);
                        if (dayOfWeek == default)
                        {
                            throw new Exception();
                        }
                        //Kiểm tra khung giờ

                        //Duyệt theo danh sách khung giờ mới
                        foreach (var tf in timeFrames)
                        {
                            var timeFrame = unitOfWork.TimeFrames.Get(tf);

                            //lấy ra lịch chiếu phim cũ

                            var movieCalendar = unitOfWork.MovieCalendars
                                .FirstOrDefault(x => x.StartWeek == lastIp.LStartWeek
                                        && x.FilmId == lastIp.LFilmId
                                        && x.DaysOfWeekId == lastIp.LDayOfWeekId
                                        && x.MovieDisplayTypeId == lastIp.LMovieDisplayTypeId
                                        && x.CinemaRoomId == lastIp.LCinemaRoomId
                                        && x.TimeFrameId == tf);

                            // Nếu có lịch cũ thì cập nhật giá trị mới
                            if (movieCalendar != null)
                            {
                                //Khởi tạo 1 lịch chiếu phim mới
                                input.TimeFrameId = tf;
                                input.Price = price + timeFrame.Price;
                                movieCalendar.DaysOfWeekId = input.DaysOfWeekId;
                                movieCalendar.FilmId = input.FilmId;
                                movieCalendar.StartWeek = input.StartWeek;
                                movieCalendar.MovieDisplayTypeId = input.MovieDisplayTypeId;
                                movieCalendar.DateTimeDetail = input.StartWeek.AddDays(dayOfWeek.GetDayValue()).Add(timeFrame.Time);

                                //Kiểm tra lịch chiếu phim đã tồn tại chưa?
                                var isDuplicate = unitOfWork.MovieCalendars
                               .Query(x => x.StartWeek == input.StartWeek
                                       && x.FilmId == input.FilmId
                                       && x.DaysOfWeekId == input.DaysOfWeekId
                                       && x.MovieDisplayTypeId == input.MovieDisplayTypeId
                                       && x.CinemaRoomId == input.CinemaRoomId
                                       && x.TimeFrameId == tf).Any();

                                if (!isDuplicate)
                                {
                                    unitOfWork.MovieCalendars.Put(movieCalendar, movieCalendar.Id);
                                }
                            }
                            else
                            {
                                //Kiểm tra lịch chiếu phim đã tồn tại chưa?
                                var isDuplicate = unitOfWork.MovieCalendars
                                      .Query(x => x.StartWeek == input.StartWeek
                                      && x.FilmId == input.FilmId
                                      && x.DaysOfWeekId == input.DaysOfWeekId
                                      && x.MovieDisplayTypeId == input.MovieDisplayTypeId
                                      && x.CinemaRoomId == input.CinemaRoomId
                                      && x.TimeFrameId == tf).Any();
                                if (!isDuplicate)
                                {
                                    var add = new MovieCalendar
                                    {
                                        Id = Guid.NewGuid(),
                                        IsDelete = false,
                                        TimeFrameId = tf,
                                        Price = price + timeFrame.Price,
                                        DaysOfWeekId = input.DaysOfWeekId,
                                        MovieDisplayTypeId = input.MovieDisplayTypeId,
                                        StartWeek = input.StartWeek,
                                        FilmId = input.FilmId,
                                        CinemaRoomId = input.CinemaRoomId,
                                        DateTimeDetail = input.StartWeek.AddDays(dayOfWeek.GetDayValue()).Add(timeFrame.Time)
                                    };
                                    if (CheckRoomAvailability(add, unitOfWork))
                                    {
                                        unitOfWork.MovieCalendars.Add(add);
                                    }
                                    else
                                    {
                                        return Json(new
                                        {
                                            status = false,
                                            mess = $"Phòng chọn đã có lịch: {timeFrame.Time}"
                                        });
                                    }
                                }
                                else
                                {
                                    return Json(new
                                    {
                                        status = false,
                                        mess = $"Đã tồn tại lịch chiếu: Phim {film.Name} - ({input.StartWeek} - {daysOfWeek.Name} -  {timeFrame.Time})"
                                    });
                                }
                            }
                        }
                        unitOfWork.Complete();
                        return Json(new { status = true, mess = "Cập nhập thành công " });
                    }
                }
                else
                {
                    using (var unitOfWork = new UnitOfWork(new CinemaOnlineDbContext()))
                    {
                        var film = unitOfWork.Films.Get(input.FilmId);
                        if (CheckInputTimeFrames(unitOfWork, input.FilmId, timeFrames))
                        {
                            var price = GetPriceCalendar(unitOfWork, input.FilmId, input.DaysOfWeekId, input.MovieDisplayTypeId);
                            var dayOfWeek = unitOfWork.DaysOfWeeks.FirstOrDefault(dow => dow.Id == input.DaysOfWeekId);
                            if (dayOfWeek == default)
                            {
                                throw new Exception();
                            }
                            foreach (var tf in timeFrames)
                            {
                                var timeFrame = unitOfWork.TimeFrames.Get(tf);

                                var isDuplicate = unitOfWork.MovieCalendars
                                    .Query(x => x.StartWeek == input.StartWeek
                                            && x.FilmId == input.FilmId
                                            && x.DaysOfWeekId == input.DaysOfWeekId
                                            && x.MovieDisplayTypeId == input.MovieDisplayTypeId
                                            && x.CinemaRoomId == input.CinemaRoomId
                                            && x.TimeFrameId == tf).Any();
                                if (!isDuplicate)
                                {
                                    var add = new MovieCalendar
                                    {
                                        Id = Guid.NewGuid(),
                                        IsDelete = false,
                                        TimeFrameId = tf,
                                        Price = price + timeFrame.Price,
                                        DaysOfWeekId = input.DaysOfWeekId,
                                        MovieDisplayTypeId = input.MovieDisplayTypeId,
                                        StartWeek = input.StartWeek,
                                        FilmId = input.FilmId,
                                        CinemaRoomId = input.CinemaRoomId,
                                        DateTimeDetail = input.StartWeek.AddDays(dayOfWeek.GetDayValue()).Add(timeFrame.Time)
                                    };
                                    if (CheckRoomAvailability(add, unitOfWork))
                                    {
                                        unitOfWork.MovieCalendars.Add(add);
                                    }
                                    else
                                    {
                                        return Json(new
                                        {
                                            status = false,
                                            mess = $"Phòng chọn đã có lịch: {timeFrame.Time}"
                                        });
                                    }
                                }
                                else
                                {
                                    return Json(new
                                    {
                                        status = false,
                                        mess = $"Đã tồn tại lịch chiếu: {timeFrame.Time}"
                                    });
                                }
                            }
                            unitOfWork.Complete();
                            return Json(new { status = true, mess = "Thêm thành công " + KeyElement });
                        }
                        else
                        {
                            return Json(new
                            {
                                status = false,
                                mess = $"Thời gian k hợp lệ - Thời lượng film là: {film.Time}"
                            });
                        }
                    }
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
        public JsonResult Del(MovieCalendarGet objectId)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(new CinemaOnlineDbContext()))
                {
                    var listCurrentTimeFrame = unitOfWork.MovieCalendars
                               .Query(x => x.StartWeek == objectId.LStartWeek
                                       && x.FilmId == objectId.LFilmId
                                       && x.DaysOfWeekId == objectId.LDayOfWeekId
                                       && x.MovieDisplayTypeId == objectId.LMovieDisplayTypeId).Select(x => x.TimeFrameId).ToList();

                    foreach (var movieCalendar in listCurrentTimeFrame.Select(tf => unitOfWork.MovieCalendars
                        .FirstOrDefault(x => x.StartWeek == objectId.LStartWeek
                                             && x.FilmId == objectId.LFilmId
                                             && x.DaysOfWeekId == objectId.LDayOfWeekId
                                             && x.MovieDisplayTypeId == objectId.LMovieDisplayTypeId
                                             && x.TimeFrameId == tf)))
                    {
                        if (movieCalendar != null)
                        {
                            //del film
                            movieCalendar.IsDelete = true;
                            movieCalendar.DeletetionTime = DateTime.Now;
                            movieCalendar.DeleterId = GetCurrentUser().Id;
                            unitOfWork.MovieCalendars.Put(movieCalendar, movieCalendar.Id);
                        }
                        else
                        {
                            return Json(new { status = false, mess = "Không tồn tại " + KeyElement });
                        }
                    }
                    unitOfWork.Complete();
                    return Json(new { status = true, mess = "Xóa thành công " + KeyElement });
                }
            }
            catch
            {
                return Json(new { status = false, mess = "Thất bại" });
            }
        }

        public JsonResult GetPrice(MovieCalendar input, List<Guid> timeFrames)
        {
            if (timeFrames.Count == 0)
            {
                return Json(new
                {
                    status = false,
                    mess = ""
                });
            }
            try
            {
                using (var unitOfWork = new UnitOfWork(new CinemaOnlineDbContext()))
                {
                    var film = unitOfWork.Films.Get(input.FilmId);
                    var daysOfWeek = unitOfWork.DaysOfWeeks.Get(input.DaysOfWeekId);

                    var movieDisplayType = unitOfWork.MovieDisplayTypes.Get(input.MovieDisplayTypeId);
                    var total = film.Price + daysOfWeek.Price + movieDisplayType.Price;

                    var html = "";
                    var i = 1;
                    foreach (var timeFrame in timeFrames.Select(item => unitOfWork.TimeFrames.Get(item)))
                    {
                        html += "<tr>"
                                + $"<td class='text-center'>{i}</td>"
                                + $"<td class='text-center'>{film.Name} - ({film.Price:0,0})</td>"
                                + $"<td class='text-center'>{daysOfWeek.Name} - ({daysOfWeek.Price:0,0})</td>"
                                + $"<td class='text-center'>{movieDisplayType.Name} - ({movieDisplayType.Price:0,0}) </td>"
                                + $"<td class='text-center'>{timeFrame.Time} - ({timeFrame.Price:0,0})</td>"
                                + $"<td class='text-center'>{total + timeFrame.Price:0,0} đ</td>"
                                + "</tr>";
                        i++;
                    }

                    return Json(new
                    {
                        html,
                        status = true,
                        mess = ""
                    });
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

        public double GetPriceCalendar(UnitOfWork unitOfWork, Guid filmId, Guid daysOfWeekId, Guid movieDisplayTypeId)
        {
            var film = unitOfWork.Films.Get(filmId);
            var daysOfWeek = unitOfWork.DaysOfWeeks.Get(daysOfWeekId);
            var movieDisplayType = unitOfWork.MovieDisplayTypes.Get(movieDisplayTypeId);

            return film.Price + daysOfWeek.Price + movieDisplayType.Price;
        }

        public bool CheckInputTimeFrames(UnitOfWork unitOfWork, Guid filmId, List<Guid> timeFrames)
        {
            var film = unitOfWork.Films.Get(filmId);

            var initTimeFrames = timeFrames.Select(tf => unitOfWork.TimeFrames.Get(tf)).ToList();

            var sortTimeFrames = initTimeFrames.OrderBy(x => x.Time).ToList();

            if (sortTimeFrames.Count <= 0) return false;
            var currentItem = sortTimeFrames[0].Time;
            for (var i = 1; i < sortTimeFrames.Count - 1; i++)
            {
                if (currentItem + film.Time > sortTimeFrames[i].Time)
                {
                    return false;
                }

                currentItem = sortTimeFrames[i].Time;
            }
            return true;
        }

        public bool CheckRoomAvailability(MovieCalendar input, UnitOfWork unitOfWork)
        {
            var movieCalendars = unitOfWork.MovieCalendars.Query(x => x.StartWeek == input.StartWeek).ToList();
            if (!movieCalendars.Any())
            {
                return true;
            }
            else
            {
                //Trong 1 tuần
                var movieCalendarsByDays = movieCalendars.Where(x => x.DaysOfWeekId == input.DaysOfWeekId).ToList();
                if (!movieCalendars.Any())
                {
                    return true;
                }
                else
                {
                    //Trong 1 ngày
                    //1. Phòng chiếu

                    var movieCalendarsByDayAndRoom = movieCalendarsByDays.Where(x => x.CinemaRoomId == input.CinemaRoomId);
                    if (!movieCalendarsByDayAndRoom.Any())
                    {
                        return true;
                    }
                    else
                    {
                        //Trong 1 giờ
                        //1. Phòng chiếu
                        var movieCalendarsByDayAndTimes = movieCalendarsByDays.Where(x => x.TimeFrameId == input.TimeFrameId).ToList();

                        if (movieCalendarsByDayAndTimes.Any())
                        {
                            //có 1 khung giờ rồi thì trùng
                            return false;
                        }
                        else
                        {
                            //Chưa có khung giờ đó nhưng cần check thời gian chiếu phim
                            // Ví dụ lấy ra time là 7:00 thì thời gian cần check cần cộng thêm tgian của phim
                            // 7:00 + 1:30:20s = 8:30:20 ////7:00 7:30 7:45 8:45 - 7:00 7:30 7:45
                            // Lịch chiếu: 7:00 7:30 7:45
                            // Khả dụng :
                            // K khả dụng: 7:00 7:30 7:45 8:45

                            var timeFrameIds = movieCalendarsByDays.Select(x => x.TimeFrameId);
                            var initTimeFrames = timeFrameIds.Select(tf => unitOfWork.TimeFrames.Get(tf)).ToList();
                            if (initTimeFrames.Count == 0)
                            {
                                return true;
                            }

                            var timeFrameInput = unitOfWork.TimeFrames.Get(input.TimeFrameId);
                            var filmInput = unitOfWork.Films.Get(input.FilmId);

                            // var curentTimeFrame = initTimeFrames[0].Time;
                            foreach (var t in initTimeFrames)
                            {
                                if (t.Time < timeFrameInput.Time)
                                {
                                    var movie = movieCalendarsByDays.FirstOrDefault(x => x.TimeFrameId == t.Id);

                                    if (movie == null) continue;
                                    var getFilmId = movie.FilmId;
                                    var getFilm = unitOfWork.Films.Get(getFilmId);

                                    if (t.Time + getFilm.Time > timeFrameInput.Time)
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    //Giờ thêm vào nhỏ hơn giờ có trong lịch

                                    if (timeFrameInput.Time + filmInput.Time > t.Time)
                                    {
                                        return false;
                                    }
                                }
                            }
                            return true;
                        }
                    }
                }
            }
        }
    }
}