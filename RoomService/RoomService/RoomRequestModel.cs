using RoomService.RoomModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoomService.RoomService
{
    public class RoomRequestModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int Number { get; set; }
        [Required]
        public int NumberOfPeople { get; set; }
        [Required]
        public decimal PriceForNight { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public RoomType RoomType { get; set; }
    }
}
