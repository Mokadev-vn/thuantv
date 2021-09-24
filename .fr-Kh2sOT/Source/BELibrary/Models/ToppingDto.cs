using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BELibrary.Constant;

namespace BELibrary.Models
{
    public class ToppingDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string KindOfTopping { get; set; }
        public int KindOfToppingEnum { get; set; }
        public string ImageUrl { get; set; }
    }
}