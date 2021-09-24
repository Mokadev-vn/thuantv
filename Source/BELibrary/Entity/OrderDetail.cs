namespace BELibrary.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrderDetail")]
    public partial class OrderDetail
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public Guid SeatId { get; set; }

        [StringLength(4)]
        public string SeatName { get; set; }

        public bool IsDelete { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DeleteTime { get; set; }

        public Guid? DeleterId { get; set; }

        public double SeatPrice { get; set; }

        public double FilmPrice { get; set; }

        public virtual Order Order { get; set; }

        public virtual Seat Seat { get; set; }
    }
}