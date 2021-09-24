namespace BELibrary.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Promotion")]
    public partial class Promotion
    {
        public Guid Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CreationTime { get; set; }

        public Guid? CreaterId { get; set; }

        public bool IsDelete { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DeletetionTime { get; set; }

        public Guid? DeleterId { get; set; }

        [StringLength(50)]
        public string KindOfPromotion { get; set; }

        public int KindOfPromotionEnum { get; set; }

        public double Price { get; set; }

        public double? PromotionPercent { get; set; }

        public bool IsFilm { get; set; }

        public Guid? FilmId { get; set; }

        [StringLength(20)]
        public string Code { get; set; }

        public bool IsActive { get; set; }

        public int Quantity { get; set; }

        public Guid? ToppingId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual Film Film { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual Topping Topping { get; set; }
    }
}