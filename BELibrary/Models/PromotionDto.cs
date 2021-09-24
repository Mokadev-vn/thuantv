using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BELibrary.Models
{
    public class PromotionDto
    {
        public Guid Id { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid CreaterId { get; set; }
        public string KindOfPromotion { get; set; }
        public int KindOfPromotionEnum { get; set; }
        public double Price { get; set; }
        public double PromotionPercent { get; set; }
        public bool IsFilm { get; set; }
        public Guid? FilmId { get; set; }
        public Guid? ToppingId { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public int Quantity { get; set; }
    }
}