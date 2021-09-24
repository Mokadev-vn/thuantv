namespace BELibrary.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
            ToppingOrderDetails = new HashSet<ToppingOrderDetail>();
        }

        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid? PromotionFilmId { get; set; }

        public double? PromotionFilmPrice { get; set; }

        public Guid? PromotionToppingId { get; set; }

        public double? PromotionToppingPrice { get; set; }

        public Guid MovieCalendarId { get; set; }

        public string FilmName { get; set; }

        public string FilmType { get; set; }

        public string RoomName { get; set; }

        public Guid FilmId { get; set; }

        public double FilmPrice { get; set; }

        public double DayOfWeekPrice { get; set; }

        public double TimeFramePrice { get; set; }

        public DateTime Time { get; set; }

        public double MovieDisplayTypePrice { get; set; }

        public string TicketNumber { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreationTime { get; set; }

        public double TotalPrice { get; set; }

        public int Status { get; set; }

        [StringLength(50)]
        public string Credits { get; set; }

        public int TicketQuantity { get; set; }

        public int PaymentType { get; set; }

        public bool IsDelete { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DeleteTime { get; set; }

        public Guid? DeleterId { get; set; }

        public virtual MovieCalendar MovieCalendar { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ToppingOrderDetail> ToppingOrderDetails { get; set; }
    }
}