using BELibrary.Core.Entity;
using BELibrary.DbContext;
using BELibrary.Entity;
using BELibrary.Utils;
using CinemaOnline.Handler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BELibrary.Core.Utils;
using CinemaOnline.Areas.Admin.Models;
using LoginModel = CinemaOnline.Models.LoginModel;

namespace CinemaOnline.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            if (!CookiesManage.Logined())
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
                {
                    var user = CookiesManage.GetUser();

                    //lấy danh sách order học được user hiện tại đăng ký
                    var orders = workScope.Orders.GetAll().Where(x => x.UserId == user.Id);

                    List<OrderView> results = new List<OrderView>();
                    foreach (var ord in orders)
                    {
                        var order = workScope.Orders.FirstOrDefault(x => x.Id == ord.Id);
                        var orderDetails = workScope.OrderDetails.Query(x => x.OrderId == order.Id);
                        var movieCalendar = workScope.MovieCalendars.FirstOrDefault(x => x.Id == order.MovieCalendarId);
                        var room = workScope.CinemaRooms.FirstOrDefault(x => x.Id == movieCalendar.CinemaRoomId);
                        var movieDisplayType = workScope.MovieDisplayTypes.FirstOrDefault(x => x.Id == movieCalendar.MovieDisplayTypeId);
                        var toppingOrders = workScope.ToppingOrders.Include(x => x.Topping).Where(x => x.OrderId == order.Id);
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
                        results.Add(result);
                    }
                    return View(results);
                }
            }
            catch (Exception e)
            {
                return Redirect("/Home/Error?mess=" + e.Message);
            }
        }

        public ActionResult Edit()
        {
            if (!CookiesManage.Logined())
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var user = CookiesManage.GetUser();

                using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
                {
                    var account = workScope.Users.GetAll().Where(x => x.Username.Trim().ToLower() == user.Username.Trim().ToLower());
                    return View(account);
                }
            }
        }

        public ActionResult Login(string returnUrl = "")
        {
            if (CookiesManage.Logined())
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateInput(true)]
        public JsonResult CheckLogin(LoginModel model)
        {
            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var account = workScope.Users.ValidFEAccount(model.Username, model.Password);

                if (HttpContext.Request.Url != null)
                {
                    var host = HttpContext.Request.Url.Authority;
                    if (account != null)
                    {
                        //đăng nhập thành công
                        var cookieClient = account.Username + "|" + host.ToLower() + "|" + account.Id;
                        var decodeCookieClient = CryptorEngine.Encrypt(cookieClient, true);
                        var userCookie = new HttpCookie(CookiesKey.Client)
                        {
                            Value = decodeCookieClient,
                            Expires = DateTime.Now.AddDays(30)
                        };
                        HttpContext.Response.Cookies.Add(userCookie);
                        return Json(new { status = true, mess = "Đăng nhập thành công" });
                    }
                    else
                    {
                        return Json(new { status = false, mess = "Tên và mật khẩu không chính xác" });
                    }
                }
                else
                {
                    return Json(new { status = false, mess = "Tên và mật khẩu không chính xác" });
                }
            }
        }

        [HttpPost]
        [ValidateInput(true)]
        public JsonResult Register(User us, string rePassword)
        {
            if (us.Password != rePassword)
            {
                return Json(new { status = false, mess = "Mật khẩu không khớp" });
            }
            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var account = workScope.Users.FirstOrDefault(x => x.Username.ToLower() == us.Username.ToLower());
                if (account == null)
                {
                    try
                    {
                        var passwordFactory = us.Password + VariableExtensions.KeyCryptorClient;
                        var passwordCryptor = CryptorEngine.Encrypt(passwordFactory, true);

                        us.IsDelete = false;
                        us.Password = passwordCryptor;
                        us.LinkAvata = us.Gender ? "/Content/images/team/2.png" : "/Content/images/team/3.png";
                        us.Id = Guid.NewGuid();
                        workScope.Users.Add(us);
                        workScope.Complete();

                        //Login luon
                        if (HttpContext.Request.Url != null)
                        {
                            var host = HttpContext.Request.Url.Authority;

                            var cookieClient = us.Username + "|" + host.ToLower() + "|" + us.Id;
                            var decodeCookieClient = CryptorEngine.Encrypt(cookieClient, true);
                            var userCookie = new HttpCookie(CookiesKey.Client)
                            {
                                Value = decodeCookieClient,
                                Expires = DateTime.Now.AddDays(30)
                            };
                            HttpContext.Response.Cookies.Add(userCookie);
                            return Json(new { status = true, mess = "Đăng ký thành công" });
                        }
                        else
                        {
                            return Json(new { status = false, mess = "Thêm không thành công" });
                        }
                    }
                    catch (Exception)
                    {
                        return Json(new { status = false, mess = "Thêm không thành công" });
                    }
                }
                else
                {
                    return Json(new { status = false, mess = "Username không khả dụng" });
                }
            }
        }

        [HttpPost]
        [ValidateInput(true)]
        public JsonResult Update(User us, HttpPostedFileBase avataUpload)
        {
            if (!CookiesManage.Logined())
            {
                return Json(new { status = false, mess = "Chưa đăng nhập" });
            }
            var user = CookiesManage.GetUser();
            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var account = workScope.Users.FirstOrDefault(x => x.Username.ToLower() == user.Username.ToLower());
                if (account != null)
                {
                    try
                    {
                        if (avataUpload?.FileName != null)
                        {
                            if (avataUpload.ContentLength >= FileKey.MaxLength)
                            {
                                return Json(new { status = false, mess = L.T("FileMaxLength") });
                            }
                            var splitFilename = avataUpload.FileName.Split('.');
                            if (splitFilename.Length > 1)
                            {
                                var fileExt = splitFilename[splitFilename.Length - 1];

                                //Check ext

                                if (FileKey.FileExtensionApprove().Any(x => x == fileExt))
                                {
                                    var slugName = StringHelper.ConvertToAlias(account.FullName);
                                    var fileName = slugName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." + fileExt;
                                    var path = Path.Combine(Server.MapPath("~/FileUploads/images/avatas/"), fileName);
                                    avataUpload.SaveAs(path);
                                    us.LinkAvata = "/FileUploads/images/avatas/" + fileName;
                                }
                                else
                                {
                                    return Json(new { status = false, mess = L.T("FileExtensionReject") });
                                }
                            }
                            else
                            {
                                return Json(new { status = false, mess = L.T("FileExtensionReject") });
                            }
                        }

                        us.Password = account.Password;
                        us.Username = account.Username;
                        us.Id = account.Id;

                        if (string.IsNullOrEmpty(us.LinkAvata))
                        {
                            us.LinkAvata = us.Gender ? "/Content/images/team/2.png" : "/Content/images/team/3.png";
                        }
                        account = us;
                        workScope.Users.Put(account, account.Id);
                        workScope.Complete();

                        //Đăng xuất
                        var nameCookie = Request.Cookies[CookiesKey.Client];
                        if (nameCookie != null)
                        {
                            var myCookie = new HttpCookie(CookiesKey.Client)
                            {
                                Expires = DateTime.Now.AddDays(-1d)
                            };
                            Response.Cookies.Add(myCookie);
                        }

                        //Login luon
                        if (HttpContext.Request.Url != null)
                        {
                            var host = HttpContext.Request.Url.Authority;

                            var cookieClient = account.Username + "|" + host.ToLower() + "|" + account.Id;
                            var decodeCookieClient = CryptorEngine.Encrypt(cookieClient, true);
                            var userCookie = new HttpCookie(CookiesKey.Client)
                            {
                                Value = decodeCookieClient,
                                Expires = DateTime.Now.AddDays(30)
                            };
                            HttpContext.Response.Cookies.Add(userCookie);
                            return Json(new { status = true, mess = "Cập nhật thành công" });
                        }
                        else
                        {
                            return Json(new { status = false, mess = "Cập nhật K thành công" });
                        }
                    }
                    catch (Exception ex)
                    {
                        return Json(new { status = false, mess = "Cập nhật không thành công", ex });
                    }
                }
                else
                {
                    return Json(new { status = false, mess = "Tài khoản không khả dụng" });
                }
            }
        }

        [HttpPost]
        [ValidateInput(true)]
        public JsonResult UpdatePass(string oldPassword, string newPassword, string rePassword)
        {
            if (oldPassword == "" || newPassword == "" || rePassword == "")
            {
                return Json(new { status = false, mess = "Không được để trống" });
            }
            if (!CookiesManage.Logined())
            {
                return Json(new { status = false, mess = "Chưa đăng nhập" });
            }
            if (newPassword != rePassword)
            {
                return Json(new { status = false, mess = "Mật khẩu không khớp" });
            }
            var user = CookiesManage.GetUser();
            using (var workScope = new UnitOfWork(new CinemaOnlineDbContext()))
            {
                var account = workScope.Users.FirstOrDefault(x => x.Username.ToLower() == user.Username.ToLower());
                if (account != null)
                {
                    try
                    {
                        var passwordFactory = oldPassword + VariableExtensions.KeyCryptorClient;
                        var passwordCryptor = CryptorEngine.Encrypt(passwordFactory, true);

                        if (passwordCryptor == account.Password)
                        {
                            passwordFactory = newPassword + VariableExtensions.KeyCryptorClient;
                            passwordCryptor = CryptorEngine.Encrypt(passwordFactory, true);

                            account.Password = passwordCryptor;
                            workScope.Users.Put(account, account.Id);
                            workScope.Complete();

                            //Đăng xuất
                            var nameCookie = Request.Cookies[CookiesKey.Client];
                            if (nameCookie != null)
                            {
                                var myCookie = new HttpCookie(CookiesKey.Client)
                                {
                                    Expires = DateTime.Now.AddDays(-1d)
                                };
                                Response.Cookies.Add(myCookie);
                            }

                            //Login luon
                            if (HttpContext.Request.Url != null)
                            {
                                var host = HttpContext.Request.Url.Authority;

                                var cookieClient = account.Username + "|" + host.ToLower() + "|" + account.Id;
                                var decodeCookieClient = CryptorEngine.Encrypt(cookieClient, true);
                                var userCookie = new HttpCookie(CookiesKey.Client)
                                {
                                    Value = decodeCookieClient,
                                    Expires = DateTime.Now.AddDays(30)
                                };
                                HttpContext.Response.Cookies.Add(userCookie);
                                return Json(new { status = true, mess = "Cập nhật thành công" });
                            }
                            else
                            {
                                return Json(new { status = false, mess = "Cập nhật K thành công" });
                            }
                        }
                        else
                        {
                            return Json(new { status = false, mess = "mật khẩu cũ không đúng" });
                        }
                    }
                    catch (Exception ex)
                    {
                        return Json(new { status = false, mess = "Cập nhật không thành công", ex });
                    }
                }
                else
                {
                    return Json(new { status = false, mess = "Tài khoản không khả dụng" });
                }
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            var nameCookie = Request.Cookies[CookiesKey.Client];
            if (nameCookie == null) return RedirectToAction("Index", "Home");
            var myCookie = new HttpCookie(CookiesKey.Client)
            {
                Expires = DateTime.Now.AddDays(-1d)
            };
            Response.Cookies.Add(myCookie);
            return RedirectToAction("Index", "Home");
        }
    }
}