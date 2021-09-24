namespace BELibrary.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Comment")]
    public partial class Comment
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public Guid FilmId { get; set; }

        public string Detail { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CreationTime { get; set; }

        public bool IsDelete { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DeletetionTime { get; set; }

        public Guid? DeleterId { get; set; }

        public virtual Film Film { get; set; }

        public virtual User User { get; set; }
    }
}