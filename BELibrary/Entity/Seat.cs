namespace BELibrary.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Seat")]
    public partial class Seat
    {
        public Guid Id { get; set; }

        [StringLength(2)]
        public string Row { get; set; }

        [StringLength(2)]
        public string Col { get; set; }

        public int Status { get; set; }

        public string Color { get; set; }

        public double Price { get; set; }

        public Guid? SeatTypeId { get; set; }

        public Guid? CinemaRoomId { get; set; }

        public bool IsDelete { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DeletetionTime { get; set; }

        public Guid? DeleterId { get; set; }

        public virtual CinemaRoom CinemaRoom { get; set; }

        public virtual SeatType SeatType { get; set; }
    }
}