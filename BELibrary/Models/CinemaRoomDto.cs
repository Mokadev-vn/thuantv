using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BELibrary.Models
{
    public class CinemaRoomDto
    {
        public Guid? Id { get; set; }
        public bool IsEdit { get; set; }
        public string Name { get; set; }
        public int? RowQuantity { get; set; }
        public double? Area { get; set; }
        public int? SeatQuantity { get; set; }
        public int? Status { get; set; }
        public string SoundQuality { get; set; }
        public List<SeatDto> Seats { get; set; }
    }

    public class SeatDto
    {
        public Guid? Id { get; set; }
        public string Row { get; set; }
        public string Col { get; set; }
        public double Price { get; set; }
        public int? Status { get; set; }
        public string Color { get; set; }
        public bool? IsDelete { get; set; }
        public Guid? SeatTypeId { get; set; }
        public Guid? CinemaRoomId { get; set; }
    }

    public class SeatTypeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Color { get; set; }
        public bool? IsDelete { get; set; }
    }
}
