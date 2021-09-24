namespace BELibrary.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MovieCalendar")]
    public partial class MovieCalendar
    {
        public Guid Id { get; set; }

        public Guid FilmId { get; set; }

        public Guid DaysOfWeekId { get; set; }

        public Guid TimeFrameId { get; set; }

        public Guid CinemaRoomId { get; set; }

        public Guid MovieDisplayTypeId { get; set; }

        public double Price { get; set; }
        public DateTime StartWeek { get; set; }

        public bool IsDelete { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DeletetionTime { get; set; }

        public Guid? DeleterId { get; set; }

        public DateTime DateTimeDetail { get; set; }

        public virtual DaysOfWeek DaysOfWeek { get; set; }

        public virtual Film Film { get; set; }

        public virtual CinemaRoom CinemaRoom { get; set; }

        public virtual MovieDisplayType MovieDisplayType { get; set; }

        public virtual TimeFrame TimeFrame { get; set; }
    }
}