using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BELibrary.Entity;

namespace CinemaOnline.Areas.Admin.Models
{
    public class OrderView
    {
        public string TicketNumber { get; set; }

        public int Status { get; set; }

        public string CreateDate { get; set; }

        //User Info
        public string FullName { get; set; }

        public string Gender { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        //Film Info
        public string FilmName { get; set; }

        public string FilmType { get; set; }

        public string RoomName { get; set; }

        public string Time { get; set; }

        public string Price { get; set; }

        public string TotalPriceFilm { get; set; }

        public string TotalPriceTopping { get; set; }

        public string TotalPricePromotion { get; set; }

        public string PromotionFilmCode { get; set; }

        public string PromotionFilmType { get; set; }

        public string PromotionFilmValue { get; set; }

        public string PromotionToppingCode { get; set; }

        public string PromotionToppingType { get; set; }

        public string PromotionToppingValue { get; set; }

        //
        public string TotalPrice { get; set; }

        public List<SeatOrder> OrderDetails { get; set; }

        public List<ToppingOrder> ToppingOrderDetails { get; set; }
    }

    public class SeatOrder
    {
        public string SeatName { get; set; }

        public string Price { get; set; }
    }

    public class ToppingOrder
    {
        public string ToppingName { get; set; }

        public string Price { get; set; }

        public int Quantity { get; set; }

        public string Total { get; set; }
    }
}