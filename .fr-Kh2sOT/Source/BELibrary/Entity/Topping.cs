namespace BELibrary.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Topping")]
    public partial class Topping
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Topping()
        {
            ToppingOrderDetails = new HashSet<ToppingOrderDetail>();
        }

        public Guid Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public double Price { get; set; }

        public string ImageUrl { get; set; }

        [StringLength(50)]
        public string KindOfTopping { get; set; }

        public int? KindOfToppingEnum { get; set; }

        public bool IsDelete { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DeletetionTime { get; set; }

        public Guid? DeleterId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ToppingOrderDetail> ToppingOrderDetails { get; set; }
    }
}