using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Model
{
    public class ReceiveReservation
    {
        public Guid RoomId { get; set; }
        public DateTime ReservStartDate { get; set; }
        public DateTime ReservFinishedDate { get; set; }
        public int NumberOfNights { get; set; }
        public Decimal AmountPaid { get; set; }
    }
}
