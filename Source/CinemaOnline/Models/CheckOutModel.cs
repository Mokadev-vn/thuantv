using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CinemaOnline.Models
{
    public class CheckOutModel
    {
        public bool Status { get; set; }

        public string Mess { get; set; }

        public double TotalPrice { get; set; }

        public Guid OrderId { get; set; }
    }
}