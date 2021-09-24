using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BELibrary
{
    public static class ErrorCodes
    {
        public class NotFound
        {
            public static string FilmMovieType = "Loại phim không tồn tại";
            public static string FilmMovieDisplayType = "Loại trình chiếu không tồn tại";
            public static string Topping = "Topping không tồn tại";
            public static string SeatType = "Loại ghế không tồn tại";
            public static string TimeFrame = "Khung giờ không tồn tại";
            public static string Promotion = "Khuyễn mãi không tồn tại";
            public static string News = "Tin tức không tồn tại";
            public static string DaysOfWeek = "Thứ không tồn tại";
        }
    }
}