namespace BELibrary.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class News
    {
        public int Id { get; set; }

        public string Title { get; set; }

        [Column(TypeName = "text")]
        public string Detail { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CreationTime { get; set; }

        public Guid CreaterId { get; set; }

        [StringLength(250)]
        public string BannerUrl { get; set; }

        public bool IsDelete { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DeletetionTime { get; set; }

        public Guid? DeleterId { get; set; }

        public virtual Admin Creater { get; set; }
    }
}