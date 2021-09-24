using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CinemaOnline.Models
{
    public class BookFinalResponse
    {
        public bool Status { get; set; }

        public string Message { get; set; }

        public string Email { get; set; }

        public string TicketNumber { get; set; }

        public string Time { get; set; }

        public string Room { get; set; }

        public string Price { get; set; }

        public string FilmName { get; set; }

        public string Sits { get; set; }

        public string StatusTicket { get; set; }
    }
}