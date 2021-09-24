using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BELibrary.Models
{
    public class NewsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public string BannerUrl { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid CreaterId { get; set; }
    }
}
