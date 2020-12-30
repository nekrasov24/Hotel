using Microsoft.AspNetCore.Http;
using RoomService.RoomModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoomService.RoomService
{
    public class EditRoomRequestModel
    {
        [Required(ErrorMessage = "Identification number is required")]
        public Guid Id { get; set; }
        
        [Required(ErrorMessage = "Name is required")]
        [StringLength(30)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Number is required")]
        [Range(1, 100)]
        public int Number { get; set; }
        [Required(ErrorMessage = "Number Of People is required")]
        [Range(1, 10)]
        public int NumberOfPeople { get; set; }
        [Required(ErrorMessage = "Price For Night is required")]
        [Range(1, 1000)]
        public decimal PriceForNight { get; set; }
        [Required(ErrorMessage = "description is required")]
        [StringLength(200)]
        public string Description { get; set; }
        [Required(ErrorMessage = "Room Type is required")]
        [Range(1, 4)]
        public RoomType RoomType { get; set; }
        public IFormFileCollection Images { get; set; }
        public List<Guid> ListImageId { get; set; }
    }
}
