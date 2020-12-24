using RoomService.RoomModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomService.RoomService
{
    public class RoomDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public int NumberOfPeople { get; set; }
        public decimal PriceForNight { get; set; }
        public string Description { get; set; }
        public RoomType RoomType { get; set; }
        public List<RoomImageModel> RoomImages { get; set; }
    }
}
