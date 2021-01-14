using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingService.BookingService
{
    public class ReservationDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime StartDateOfBooking { get; set; }
        public DateTime FinishDateOfBooking { get; set; }
        public DateTime ReservStartDate { get; set; }
        public DateTime ReservFinishedDate { get; set; }
    }
}
