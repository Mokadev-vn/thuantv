namespace BELibrary.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ToppingOrderDetail")]
    public partial class ToppingOrderDetail
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public Guid ToppingId { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }

        public virtual Order Order { get; set; }

        public virtual Topping Topping { get; set; }

        public bool IsDelete { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DeleteTime { get; set; }

        public Guid? DeleterId { get; set; }
    }
}