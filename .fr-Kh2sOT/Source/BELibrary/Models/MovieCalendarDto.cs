using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BELibrary.Models
{
    public class MovieCalendarDto
    {
        public Guid Id { get; set; }

        public Guid FilmId { get; set; }

        public string FilmName { get; set; }

        public Guid DaysOfWeekId { get; set; }

        public string DaysOfWeekName { get; set; }

        public Guid TimeFrameId { get; set; }

        public string TimeFrameName { get; set; }

        public Guid MovieDisplayTypeId { get; set; }

        public string MovieDisplayTypeName { get; set; }

        public DateTime StartWeek { get; set; }

        public double? Price { get; set; }
    }
}