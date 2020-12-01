using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomService.RoomModel
{
    public class Room
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public int NumberOfPeople { get; set; }
        public decimal PriceForNight { get; set; }




    }
}
