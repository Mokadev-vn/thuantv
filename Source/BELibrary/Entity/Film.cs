namespace BELibrary.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Film")]
    public partial class Film
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Film()
        {
            Comments = new HashSet<Comment>();
            FilmMovieTypes = new HashSet<FilmMovieType>();
            FilmMovieDisplayTypes = new HashSet<FilmMovieDisplayType>();
            MovieCalendars = new HashSet<MovieCalendar>();
        }

        public Guid Id { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        public string TrailerLink { get; set; }

        public string Image { get; set; }

        public string Actors { get; set; }

        public int AgeRestriction { get; set; }

        public DateTime ReleaseDate { get; set; }

        [StringLength(50)]
        public string Directors { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        public TimeSpan? Time { get; set; }

        [StringLength(250)]
        public string BannerUrl { get; set; }

        [StringLength(50)]
        public string TimeComing { get; set; }

        public string Detail { get; set; }

        public double Price { get; set; }

        public bool IsDelete { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DeletetionTime { get; set; }

        public Guid? DeleterId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FilmMovieType> FilmMovieTypes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FilmMovieDisplayType> FilmMovieDisplayTypes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MovieCalendar> MovieCalendars { get; set; }
    }
}