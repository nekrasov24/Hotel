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
        [Required(ErrorMessage = "Name is required", AllowEmptyStrings = false)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Number is required")]
        public int Number { get; set; }
        [Required(ErrorMessage = "Number Of People is required")]
        public int NumberOfPeople { get; set; }
        [Required(ErrorMessage = "Price For Night required")]
        public decimal PriceForNight { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Room Type is required")]
        public RoomType RoomType { get; set; }
    }
}
