using BELibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BELibrary.Models
{
    public class StatisticDto
    {
        public DateTime PassTime { get; set; }
        public TimeSpan TimeOfDay { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Week { get; set; }
        public StatisticStatusesEnum TypeOfStatistic { get; set; }
        public Guid? FilmId { get; set; }
    }

    public class StatisticResultDto
    {
        public StatisticDto Condition { get; set; }
        public object Result { get; set; }
    }
}
