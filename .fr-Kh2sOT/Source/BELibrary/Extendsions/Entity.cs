using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BELibrary.Entity;

namespace BELibrary.Extendsions
{
    public static class Entity
    {
        public static string GetKindOfTopping(this Topping obj)
        {
            return obj.KindOfTopping + "|" + obj.KindOfToppingEnum.ToString();
        }

        public static string GetKindOfPromotion(this Promotion obj)
        {
            return obj.KindOfPromotion + "|" + obj.KindOfPromotionEnum.ToString();
        }

        public static string GetPriceValueDisplay(this Promotion obj)
        {
            return obj.PromotionPercent == 0 ? obj.Price + " VNĐ" : obj.PromotionPercent + " %";
        }

        public static string GetTypeDisplay(this Promotion obj)
        {
            return obj.IsFilm ? "Khuyến mãi phim" : "Khuyến mãi topping";
        }

        public static string GetToppingOrFilmName(this Promotion obj)
        {
            return obj.Film == null
                    ? obj.Topping == null
                        ? "" : obj.Topping.Name
                    : obj.Film.Name;
        }

        public static string GetEnStatus(this Promotion obj)
        {
            return obj.IsActive ? "success" : "danger";
        }

        public static string GetVnStatus(this Promotion obj)
        {
            return obj.IsActive ? "Hoạt động" : "Đã hủy";
        }

        public static string GetStatusStr(this CinemaRoom obj)
        {
            return obj.Status == 0 ? "Sắp chiếu" : "Đang chiếu";
        }

        public static string GetStatusForClass(this CinemaRoom obj)
        {
            return obj.Status == 0 ? "warning" : "success";
        }

        public static int GetDayValue(this DaysOfWeek obj)
        {
            switch (obj.Name)
            {
                case "Thứ 2":
                    return 0;

                case "Thứ 3":
                    return 1;

                case "Thứ 4":
                    return 2;

                case "Thứ 5":
                    return 3;

                case "Thứ 6":
                    return 4;

                case "Thứ 7":
                    return 5;
            }
            return 6;
        }

        public static string GetTimeFrame(this TimeFrame obj)
        {
            return obj.Time.ToString(@"hh\:mm");
        }

        public static string GetSeatName(this Seat obj)
        {
            return obj.Row + obj.Col;
        }

        public static string GetVnCurrency(this double obj)
        {
            return obj.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));
        }

        public static string CheckDoubleSeat(this List<SeatType> obj, Guid Id)
        {
            return obj.FirstOrDefault(st => st.Id == Id).Name.Equals("Ghế đôi") ? "true" : "false";
        }
    }
}