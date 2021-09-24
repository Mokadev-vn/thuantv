namespace BELibrary.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CinemaRoom")]
    public partial class CinemaRoom
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CinemaRoom()
        {
            Seats = new HashSet<Seat>();
        }

        public Guid Id { get; set; }

        [StringLength(20)]
        public string Name { get; set; }

        public int RowQuantity { get; set; }

        public double? Area { get; set; }

        public int SeatQuantity { get; set; }

        public int Status { get; set; }
        [StringLength(20)]
        public string SoundQuality { get; set; }

        public bool IsDelete { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DeletetionTime { get; set; }

        public Guid? DeleterId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Seat> Seats { get; set; }
    }
}