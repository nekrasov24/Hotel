using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingService.BookingService
{
    public class BookingRequestModel
    {
        public Guid RoomId { get; set; }
        public DateTime ReservStartDate { get; set; }
        public DateTime ReservFinishedDate { get; set; }
    }
}
