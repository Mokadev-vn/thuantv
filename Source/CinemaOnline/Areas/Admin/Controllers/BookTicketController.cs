using BELibrary.Core.Entity;
using BELibrary.DbContext;
using BELibrary.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CinemaOnline.Areas.Admin.Models;

namespace CinemaOnline.Areas.Admin.Controllers
{
    public class BookTicketController : BaseController
    {
        private readonly string KeyElement = "Đặt vé";

        // GET: Admin/Gallery
        public ActionResult Index(Guid? filmId)
        {
            ViewBag.Feature = "Danh sách";
            ViewBag.Element = KeyElement;
            if (Request.Url != null) ViewBag.BaseURL = Request.Url.LocalPath;
            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var films = workScope.Films.GetAll().OrderByDescending(x => x.ReleaseDate).ToList();
                ViewBag.Films = new SelectList(films, "Id", "Name");
                if (!filmId.HasValue)
                {
                    if (films.Count > 0)
                    {
                        filmId = films[0].Id;
                    }
                }
                ViewBag.RoomId = filmId;
                var listData = workScope.Orders.Include(x => x.User).Where(x => x.FilmId == filmId).ToList();
                return View(listData);
            }
        }

        public ActionResult Detail(Guid orderId)
        {
            ViewBag.Feature = "Chi tiết";
            ViewBag.Element = KeyElement;
            if (Request.Url != null)
                ViewBag.BaseURL = string.Join("", Request.Url.Segments.Take(Request.Url.Segments.Length - 1));
            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var order = workScope.Orders.FirstOrDefault(x => x.Id == orderId);
                var orderDetails = workScope.OrderDetails.Query(x => x.OrderId == order.Id);
                var movieCalendar = workScope.MovieCalendars.FirstOrDefault(x => x.Id == order.MovieCalendarId);
                var room = workScope.CinemaRooms.FirstOrDefault(x => x.Id == movieCalendar.CinemaRoomId);
                var movieDisplayType = workScope.MovieDisplayTypes.FirstOrDefault(x => x.Id == movieCalendar.MovieDisplayTypeId);
                var toppingOrders = workScope.ToppingOrders.Include(x => x.Topping).Where(x => x.OrderId == order.Id);
                var user = workScope.Users.FirstOrDefault(x => x.Id == order.UserId);
                var promotionFilm = workScope.Promotions.FirstOrDefault(x => x.Id == order.PromotionFilmId);
                var promotionValue = "";
                if (promotionFilm != null)
                {
                    switch (promotionFilm.KindOfPromotionEnum)
                    {
                        // Đồng giá
                        case 0:
                            promotionValue = $"{promotionFilm.Price} đ";
                            break;
                        //Trên phim
                        case 1:
                            promotionValue = $"{promotionFilm.PromotionPercent} %";
                            break;
                        //Trên hóa đơn
                        case 2:
                            promotionValue = $"{promotionFilm.PromotionPercent} %";
                            break;
                    }
                }

                var listOrderDetail = orderDetails as OrderDetail[] ?? orderDetails.ToArray();
                var seats = listOrderDetail.Select(seat =>
                    new SeatOrder
                    {
                        Price = $"{(seat.FilmPrice + seat.SeatPrice):0,0} đ",
                        SeatName = seat.SeatName
                    }).ToList();
                var toppingOrderDetails = toppingOrders as ToppingOrderDetail[] ?? toppingOrders.ToArray();

                var toppings = toppingOrderDetails.Select(topping =>
                    new ToppingOrder
                    {
                        Price = $"{topping.Price:0,0} đ",
                        ToppingName = topping.Topping.Name,
                        Quantity = topping.Quantity,
                        Total = $"{topping.Price * topping.Quantity:0,0} đ"
                    }).ToList();

                var totalPriceTopping = 0.0;
                toppingOrderDetails.ToList().ForEach(x => totalPriceTopping += x.Quantity * x.Price);

                var totalPriceFilm = 0.0;
                listOrderDetail.ToList().ForEach(x => totalPriceFilm += x.FilmPrice + x.SeatPrice);

                var result = new OrderView
                {
                    TicketNumber = order.TicketNumber,
                    FilmName = order.FilmName,
                    Gender = user.Gender ? "Nam" : "Nữ",
                    FullName = user.FullName,
                    FilmType = movieDisplayType.Name,
                    Phone = user.Phone,
                    Email = user.Email,
                    Time = order.Time.ToString("g"),
                    TotalPrice = $"{order.TotalPrice:0,0} đ",
                    Price = $"{order.FilmPrice:0,0} đ",
                    RoomName = order.RoomName,
                    PromotionFilmCode = promotionFilm?.Code,
                    PromotionFilmType = promotionFilm?.KindOfPromotion,
                    PromotionFilmValue = promotionValue,
                    //PromotionToppingCode = promotionFilm?.Code,
                    //PromotionToppingType = promotionFilm?.KindOfPromotion,
                    //PromotionToppingValue = promotionValue,
                    CreateDate = order.CreationTime.ToString("g"),
                    OrderDetails = seats,
                    ToppingOrderDetails = toppings,
                    TotalPriceTopping = $"{totalPriceTopping:0,0} đ",
                    TotalPriceFilm = $"{totalPriceFilm:0,0} đ",
                    TotalPricePromotion = $"{(order.TotalPrice - (totalPriceFilm + totalPriceTopping)):0,0} đ"
                };
                ViewBag.IsEdit = false;
                return View(result);
            }
        }

        [HttpPost]
        public JsonResult Del(Guid id)
        {
            return Json(new { status = false, mess = "Đang xây dựng " + KeyElement });
            try
            {
                using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
                {
                    var elm = workScope.Orders.Get(id);
                    if (elm != null)
                    {
                        //del
                        workScope.Orders.Remove(elm);
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