using BELibrary.Entity;
using BELibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BELibrary.Models.View
{
    public class ReserveSeatView
    {
        public double Price { get; set; }        
        public CinemaRoom CinemaRoom { get; set; }
        public ReserveTicketDto CurrentBooking { get; set; }
    }

    public class SelectToppingView
    {
        public List<Topping> Toppings { get; set; }
        public ReserveTicketDto CurrentBooking { get; set; }
    }

    public class ReserveTicketDto
    {
        public string HubId { get; set; }
        public Guid UserId { get; set; }
        public Guid MovieCalendarId { get; set; }
        public List<Guid> SeatIds { get; set; }
        public List<ToppingDto> Toppings { get; set; }
        public string FilmPromotionCode { get; set; }
        public string ToppingPromotionCode { get; set; }
        public string FilmName { get; set; }
        public Guid FilmId { get; set; }
        public string StartBooking { get; set; }
        public double TotalPrice { get; set; }
        public int PaymentType { get; set; }
        public string Message { get; set; }
        public BookingStatusesEnum Status = BookingStatusesEnum.ReserveSeat;
    }

    public class ToppingDto
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }
}
