using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoomService.RoomModel
{
    public class Room
    {
        [Key]

        public Guid Id { get; set; }
        [StringLength(30)]
        public string Name { get; set; }
        [Range(1, 100)]
        public int Number { get; set; }
        [Range(1, 10)]
        public int NumberOfPeople { get; set; }
        [Range(1, 1000)]
        public decimal PriceForNight { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        public RoomType RoomType { get; set; }


    }
}
