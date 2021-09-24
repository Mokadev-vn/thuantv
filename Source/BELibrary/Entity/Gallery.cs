using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BELibrary.Entity
{
    [Table("Gallery")]
    public class Gallery
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime CreationTime { get; set; }

        [StringLength(250)]
        public string Url { get; set; }

        [StringLength(250)]
        public string UrlThumb { get; set; }

        public bool IsDelete { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DeletetionTime { get; set; }

        public Guid? DeleterId { get; set; }
    }
}