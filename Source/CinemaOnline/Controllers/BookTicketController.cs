using BELibrary.Core.Entity;
using BELibrary.Core.Utils;
using BELibrary.DbContext;
using BELibrary.Entity;
using BELibrary.Enums;
using BELibrary.Extendsions;
using BELibrary.Models.View;
using BELibrary.Utils;
using CinemaOnline.Handler;
using CinemaOnline.Handler.Payment.Momo;
using CinemaOnline.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace CinemaOnline.Controllers
{
    public class BookTicketController : BaseController
    {
        // GET: BookTicket
        public ActionResult Index()
        {
            return Redirect("/");
        }

        public ActionResult Payment(ResponsePayment responsePayment)
        {
            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                try
                {
                    var orderId = Guid.Parse(responsePayment.OrderId);
                    var order = workScope.Orders.FirstOrDefault(x => x.Id == orderId);
                    if (order == null)
                    {
                        return RedirectToAction("BookFinal", new BookFinalResponse
                        {
                            Message = "",
                            Status = false,
                        });
                    }

                    #region Send Mail

                    var path = Server.MapPath("~/FileUploads/files/template/receipt.html");
                    var pathChild = Server.MapPath("~/FileUploads/files/template/child.html");

                    var content = System.IO.File.ReadAllText(path);
                    var contentChild = System.IO.File.ReadAllText(pathChild);

                    var childSeat = "";

                    var sits = "";

                    var orderDetails = workScope.OrderDetails.Query(x => x.OrderId == order.Id);

                    // Get promotion film
                    var priceFilm = 0.0;
                    var promotionFilmPrice = 0.0;
                    var promotionInvoice = 0.0;
                    var isPromotionInvoice = false;

                    var promotionFilm = workScope.Promotions
                        .FirstOrDefault(x =>
                            x.Id == order.PromotionFilmId
                            && x.IsFilm);

                    if (promotionFilm != null)
                    {
                        switch (promotionFilm.KindOfPromotionEnum)
                        {
                            // Đồng giá
                            case 0:
                                promotionFilmPrice = promotionFilm.Price;
                                priceFilm = promotionFilm.Price;
                                break;
                            //Trên phim
                            case 1:
                                if (promotionFilm.FilmId == order.FilmId)
                                {
                                    promotionFilmPrice =
                                        order.FilmPrice - order.FilmPrice * (promotionFilm.PromotionPercent.GetValueOrDefault() / 100);
                                    priceFilm = order.FilmPrice * (promotionFilm.PromotionPercent.GetValueOrDefault() / 100) + order.MovieDisplayTypePrice + order.DayOfWeekPrice;
                                    break;
                                }
                                else
                                {
                                    priceFilm = order.FilmPrice + order.MovieDisplayTypePrice + order.DayOfWeekPrice;
                                    break;
                                }

                            //Trên hóa đơn
                            case 2:
                                promotionInvoice = promotionFilm.PromotionPercent.GetValueOrDefault();
                                isPromotionInvoice = true;
                                break;
                        }
                    }
                    else
                    {
                        priceFilm = order.FilmPrice;
                    }

                    foreach (var orderDetail in orderDetails)
                    {
                        //var childTemp = contentChild.Replace("{Name}", $"{orderDetail.SeatName}");
                        //Need update
                        var seat = workScope.Seats.Include(x => x.SeatType).FirstOrDefault(x => x.Id == orderDetail.SeatId);
                        var childTemp = contentChild.Replace("{Name}", $"{orderDetail.SeatName} - {seat?.SeatType.Name}");
                        childTemp = childTemp.Replace("{Price}", $"{(priceFilm + orderDetail.SeatPrice):0,0} đ");

                        childSeat += childTemp;
                        sits += $"{orderDetail.SeatName}, ";
                    }

                    var movieCalendar = workScope.MovieCalendars.FirstOrDefault(x => x.Id == order.MovieCalendarId);
                    var movieDisplayType = workScope.MovieDisplayTypes.FirstOrDefault(x => x.Id == movieCalendar.MovieDisplayTypeId);

                    var toppingOrders = workScope.ToppingOrders.Include(x => x.Topping).Where(x => x.OrderId == order.Id);
                    var childTopping = "";

                    foreach (var toppingOrder in toppingOrders)
                    {
                        var childTemp = contentChild.Replace("{Name}", $"{toppingOrder.Topping.Name} x {toppingOrder.Quantity}");
                        childTemp = childTemp.Replace("{Price}", $"{(toppingOrder.Price):0,0} đ");

                        childTopping += childTemp;
                    }

                    string statusTicket;
                    if (string.IsNullOrEmpty(responsePayment.ErrorCode))
                    {
                        order.Status = StatusOrder.Success;
                        statusTicket = "Thanh toán thành công";
                        workScope.Complete();
                    }
                    else
                    {
                        order.Status = StatusOrder.Failure;
                        statusTicket = "Thanh toán thất bại";
                        workScope.Complete();
                    }

                    content = content.Replace("{FilmName}", order.FilmName);
                    content = content.Replace("{Type}", movieDisplayType.Name);
                    content = content.Replace("{RoomName}", order.RoomName);
                    content = content.Replace("{Time}", movieCalendar.DateTimeDetail.ToString("MM/dd/yyyy h:mm tt"));
                    content = content.Replace("{Code}", order.TicketNumber);
                    content = content.Replace("{Status}", statusTicket);
                    content = content.Replace("{Amount}", order.TicketQuantity.ToString());
                    content = content.Replace("{ChildSeat}", childSeat);
                    content = content.Replace("{ChildTopping}", childTopping);
                    content = content.Replace("{Host}", "http://localhost:63570/");
                    content = content.Replace("{TotalPrice}", $"{order.TotalPrice:0,0} đ");

                    var user = CookiesManage.GetUser();

                    var rp = MailUtils.SendEmail(user.Email, "Đặt vé thành công", content);

                    #endregion Send Mail

                    return RedirectToAction("BookFinal", new BookFinalResponse
                    {
                        Message = $"Pay: {responsePayment.Message} - Mail: {rp.Status}",
                        Status = true,
                        Email = user.Email,
                        FilmName = order.FilmName,
                        Price = $"{double.Parse(responsePayment.Amount):0,0} đ",
                        Room = order.RoomName,
                        Sits = sits.Remove(sits.Length - 2, 2),
                        TicketNumber = order.TicketNumber,
                        Time = movieCalendar.DateTimeDetail.ToString("MM/dd/yyyy h:mm tt"),
                        StatusTicket = statusTicket
                    });
                }
                catch (Exception e)
                {
                    return RedirectToAction("BookFinal", new BookFinalResponse
                    {
                        Message = e.Message,
                        Status = false
                    });
                }
            }
        }

        public ActionResult BookFinal(BookFinalResponse rp)
        {
            return View(rp);
        }

        public ActionResult ReserveSeat(Guid movieCalendarId)
        {
            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var seatTypes = workScope.SeatTypes.GetAll().ToList();
                var movieCalendar = workScope.MovieCalendars.Include(mc => mc.Film).FirstOrDefault(mc => mc.Id == movieCalendarId);
                if (movieCalendar == default)
                {
                    throw new Exception();
                }
                var cinemaRoom = workScope.CinemaRooms
                    .IncludeFilter(cr => cr.Seats.Where(s => s.IsDelete == false))
                    .FirstOrDefault(cr => cr.IsDelete == false && cr.Id == movieCalendar.CinemaRoomId);

                var seatHasBook = new List<Guid>();

                var bookings = workScope.Orders.Query(x => x.MovieCalendarId == movieCalendar.Id);

                foreach (var booking in bookings)
                {
                    var seats = workScope.OrderDetails.Query(x => x.OrderId == booking.Id)
                        .Select(x => x.SeatId);
                    seatHasBook.AddRange(seats);
                }

                var currentBooking = new ReserveTicketDto { UserId = GetCurrentUser().Id, SeatIds = seatHasBook, MovieCalendarId = movieCalendarId, FilmName = movieCalendar.Film.Name, FilmId = movieCalendar.FilmId };
                var result = new ReserveSeatView { Price = movieCalendar.Price, CinemaRoom = cinemaRoom, CurrentBooking = currentBooking };
                ViewBag.SeatTypes = seatTypes;
                return View(result);
            }
        }

        public ActionResult SelectTopping(string jsonData)
        {
            var input = JsonConvert.DeserializeObject<ReserveTicketDto>(jsonData);
            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var toppingList = workScope.Toppings.GetAll().ToList();
                var result = new SelectToppingView { CurrentBooking = input, Toppings = toppingList };
                input.Status = BookingStatusesEnum.SelectTopping;
                return View(result);
            }
        }

        public ActionResult AddPromotion(string jsonData)
        {
            var input = JsonConvert.DeserializeObject<ReserveTicketDto>(jsonData);
            return View(input);
        }

        public ActionResult CheckPromotion(string jsonData)
        {
            var input = JsonConvert.DeserializeObject<ReserveTicketDto>(jsonData);
            using (var workSpace = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                if (!string.IsNullOrWhiteSpace(input.FilmPromotionCode))
                {
                    var pr = workSpace.Promotions.GetAll()
                    .FirstOrDefault(x => x.IsFilm && !x.IsDelete && x.Code.Equals(input.FilmPromotionCode));
                    if (pr == default)
                    {
                        input.Status = BookingStatusesEnum.Error;
                        input.Message = $"Mã giảm giá {input.FilmPromotionCode} không tồn tại";
                        return RedirectToAction("AddPromotion", "BookTicket", new { jsonData = JsonConvert.SerializeObject(input) });
                    }
                    if (pr.Quantity == 0)
                    {
                        input.Status = BookingStatusesEnum.Error;
                        input.Message = $"Mã giảm giá {input.FilmPromotionCode} đã hết";
                        return RedirectToAction("AddPromotion", "BookTicket", new { jsonData = JsonConvert.SerializeObject(input) });
                    }
                }

                if (!string.IsNullOrWhiteSpace(input.ToppingPromotionCode))
                {
                    var pr = workSpace.Promotions.GetAll()
                    .FirstOrDefault(x => !x.IsFilm && !x.IsDelete && x.Code.Equals(input.ToppingPromotionCode));
                    if (pr == default)
                    {
                        input.Status = BookingStatusesEnum.Error;
                        input.Message = $"Mã giảm giá {input.ToppingPromotionCode} không tồn tại";
                        return RedirectToAction("AddPromotion", "BookTicket", new { jsonData = JsonConvert.SerializeObject(input) });
                    }
                    if (pr.Quantity == 0)
                    {
                        input.Status = BookingStatusesEnum.Error;
                        input.Message = $"Mã giảm giá {input.ToppingPromotionCode} đã hết";
                        return RedirectToAction("AddPromotion", "BookTicket", new { jsonData = JsonConvert.SerializeObject(input) });
                    }
                }
                var result = CheckOut(input);
                input.Status = BookingStatusesEnum.BookingFinal;
                if (!result.Status)
                {
                    return Redirect("/account/login");
                }

                var orderInfo = $"[A - Movie] - {input.FilmName}";

                var host = "http://localhost:63570";
                if (Request.Url != null)
                    host = $"{Request.Url.Scheme}://{Request.Url.Authority}";

                var returnUrl = $"{host}/BookTicket/Payment";

                if (result.TotalPrice <= 0)
                {
                    return Redirect("/Home/Error?mess= Số tiền không hợp lệ");
                }

                const string notifyUrl = "http://localhost:63570/notifyUrl";
                try
                {
                    var url = MomoExtend.GenUrlPay(orderInfo, returnUrl, notifyUrl, result.TotalPrice.ToString(CultureInfo.InvariantCulture), result.OrderId.ToString());
                    return Redirect(url);
                }
                catch (Exception e)
                {
                    return Redirect("/Home/Error?mess=" + e.Message);
                }
            }
        }

        private static CheckOutModel CheckOut(ReserveTicketDto input)
        {
            //ReserveTicketDto input = JsonConvert.DeserializeObject<ReserveTicketDto>(jsonData);
            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                double totalPrice;
                Guid orderId;

                if (CookiesManage.Logined())
                {
                    var user = CookiesManage.GetUser();

                    //Get movie calendar

                    var movieCalendar = workScope.MovieCalendars.FirstOrDefault(x => x.Id == input.MovieCalendarId);
                    var film = workScope.Films.FirstOrDefault(x => x.Id == movieCalendar.FilmId);
                    var room = workScope.CinemaRooms.FirstOrDefault(x => x.Id == movieCalendar.CinemaRoomId);
                    var timeFrame = workScope.TimeFrames.FirstOrDefault(x => x.Id == movieCalendar.TimeFrameId);
                    var daysOfWeek = workScope.DaysOfWeeks.FirstOrDefault(x => x.Id == movieCalendar.DaysOfWeekId);
                    var movieDisplayType = workScope.MovieDisplayTypes.FirstOrDefault(x => x.Id == movieCalendar.MovieDisplayTypeId);

                    // Get promotion film
                    var promotionFilm = workScope.Promotions
                        .FirstOrDefault(x =>
                        x.Code == input.FilmPromotionCode
                        && x.IsFilm);

                    var priceFilm = 0.0;
                    var promotionFilmPrice = 0.0;
                    var promotionInvoice = 0.0;
                    var isPromotionInvoice = false;

                    // Get seats
                    var seats = new List<Seat>();
                    foreach (var seat in input.SeatIds.Select(seatId => workScope.Seats.FirstOrDefault(x => x.Id == seatId)))
                    {
                        if (seat == null)
                        {
                            throw new Exception();
                        }
                        else
                        {
                            seats.Add(seat);
                        }
                    }

                    #region Promotion Film

                    if (promotionFilm != null)
                    {
                        switch (promotionFilm.KindOfPromotionEnum)
                        {
                            // Đồng giá
                            case 0:
                                promotionFilmPrice = promotionFilm.Price;
                                priceFilm = promotionFilm.Price;
                                break;
                            //Trên phim
                            case 1:
                                if (promotionFilm.FilmId == movieCalendar.FilmId)
                                {
                                    promotionFilmPrice =
                                        film.Price - (film.Price - film.Price * (promotionFilm.PromotionPercent.GetValueOrDefault() / 100)) * seats.Count;
                                    priceFilm =
                                        film.Price - film.Price * (promotionFilm.PromotionPercent.GetValueOrDefault() / 100) + movieDisplayType.Price + daysOfWeek.Price + timeFrame.Price;
                                    break;
                                }
                                else
                                {
                                    priceFilm = film.Price + movieDisplayType.Price + daysOfWeek.Price + timeFrame.Price;
                                    promotionFilmPrice = 0;
                                    break;
                                }

                            //Trên hóa đơn
                            case 2:
                                priceFilm = film.Price + movieDisplayType.Price + daysOfWeek.Price + timeFrame.Price;
                                promotionInvoice = promotionFilm.PromotionPercent.GetValueOrDefault();
                                isPromotionInvoice = true;
                                break;
                        }
                    }
                    else
                    {
                        priceFilm = film.Price + movieDisplayType.Price + daysOfWeek.Price + timeFrame.Price;
                    }

                    #endregion Promotion Film

                    // Init Order
                    var order = new Order
                    {
                        Id = Guid.NewGuid(),
                        UserId = user.Id,
                        PromotionFilmId = promotionFilm?.Id,
                        MovieCalendarId = movieCalendar.Id,
                        FilmName = input.FilmName,
                        TicketNumber = CodeUtils.RandomString(8),
                        FilmId = film.Id,
                        RoomName = room.Name,
                        FilmPrice = film.Price,
                        DayOfWeekPrice = daysOfWeek.Price,
                        TimeFramePrice = timeFrame.Price,
                        MovieDisplayTypePrice = movieDisplayType.Price,
                        CreationTime = DateTime.Now,
                        Time = movieCalendar.DateTimeDetail,
                        FilmType = movieDisplayType.Name,
                        Status = StatusOrder.Pending
                    };

                    //Init Order Detail
                    var totalPriceOrderDetail = 0.0;
                    var orderDetails = new List<OrderDetail>();
                    foreach (var seat in seats)
                    {
                        var orderDetail = new OrderDetail
                        {
                            Id = Guid.NewGuid(),
                            OrderId = order.Id,
                            SeatId = seat.Id,
                            SeatName = seat.GetSeatName(),
                            SeatPrice = seat.Price,
                            FilmPrice = priceFilm
                        };
                        orderDetails.Add(orderDetail);
                        totalPriceOrderDetail += priceFilm + seat.Price;
                    }

                    // Get promotion Topping
                    var promotionTopping = workScope.Promotions
                        .FirstOrDefault(x =>
                            x.Code == input.ToppingPromotionCode
                            && !x.IsFilm);

                    var priceTopping = 0.0;
                    var promotionInvoiceTopping = 0.0;
                    var promotionPriceTopping = 0.0;
                    var isPromotionInvoiceTopping = false;
                    var isPromotionForTopping = false;

                    if (promotionTopping != null)
                    {
                        switch (promotionTopping.KindOfPromotionEnum)
                        {
                            // Đồng giá
                            case 3:
                                priceTopping = promotionTopping.Price;
                                promotionPriceTopping = promotionTopping.Price;
                                break;
                            //Trên Toping
                            case 4:
                                isPromotionForTopping = true;
                                break;

                            //Trên hóa đơn
                            case 5:
                                promotionInvoiceTopping = promotionTopping.PromotionPercent.GetValueOrDefault();
                                isPromotionInvoiceTopping = true;
                                break;
                        }
                    }

                    //Init topping Order Details
                    var totalPriceToppingOrderDetails = 0.0;
                    var toppingOrderDetails = new List<ToppingOrderDetail>();
                    foreach (var topping in input.Toppings)
                    {
                        if (promotionTopping != null)
                        {
                            if (isPromotionForTopping && promotionTopping.ToppingId == topping.Id)
                            {
                                var tempPrice = topping.Price * (promotionTopping.PromotionPercent.GetValueOrDefault() / 100);
                                priceTopping = (topping.Price - tempPrice) * topping.Quantity;
                                promotionPriceTopping = tempPrice * topping.Quantity;
                            }
                            else
                            {
                                priceTopping = topping.Quantity * topping.Price;
                                promotionPriceTopping = 0;
                            }
                        }
                        else
                        {
                            priceTopping = topping.Quantity * topping.Price;
                            promotionPriceTopping = 0;
                        }
                        var toppingOrderDetail = new ToppingOrderDetail
                        {
                            Id = Guid.NewGuid(),
                            OrderId = order.Id,
                            Price = priceTopping,
                            Quantity = topping.Quantity,
                            ToppingId = topping.Id
                        };
                        toppingOrderDetails.Add(toppingOrderDetail);
                        totalPriceToppingOrderDetails += toppingOrderDetail.Price;
                    }

                    //=========Final Data===========//
                    order.TicketQuantity = orderDetails.Count;

                    double priceInvoiceFilm;
                    double priceInvoiceTopping;

                    //calculator by promotion
                    //film
                    if (!isPromotionInvoice)
                    {
                        priceInvoiceFilm = totalPriceOrderDetail;
                    }
                    else
                    {
                        priceInvoiceFilm = totalPriceOrderDetail - (totalPriceOrderDetail * (promotionInvoice / 100));
                        promotionFilmPrice = totalPriceOrderDetail * (promotionInvoice / 100);
                    }

                    //calculator by promotion
                    //topping
                    if (!isPromotionInvoiceTopping)
                    {
                        priceInvoiceTopping = totalPriceToppingOrderDetails;
                    }
                    else
                    {
                        priceInvoiceTopping = totalPriceToppingOrderDetails - (totalPriceToppingOrderDetails * (promotionInvoiceTopping / 100));
                        promotionPriceTopping = totalPriceToppingOrderDetails * (promotionInvoiceTopping / 100);
                    }

                    order.PromotionToppingId = promotionTopping?.Id;
                    order.PromotionToppingPrice = promotionPriceTopping;

                    order.PromotionFilmPrice = promotionFilmPrice;

                    totalPrice = priceInvoiceFilm + priceInvoiceTopping;
                    order.TotalPrice = totalPrice;
                    orderId = order.Id;

                    //=========Save Data===========//

                    workScope.Orders.Add(order);
                    workScope.Complete();

                    workScope.OrderDetails.AddRange(orderDetails);
                    workScope.ToppingOrders.AddRange(toppingOrderDetails);
                    workScope.Complete();
                }
                else
                {
                    return new CheckOutModel
                    {
                        Mess = "",
                        Status = false,
                        TotalPrice = 0,
                        OrderId = new Guid()
                    };
                }
                return new CheckOutModel
                {
                    Mess = "",
                    Status = true,
                    TotalPrice = totalPrice,
                    OrderId = orderId
                };
            }
        }
    }
}