using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingService.BookingService
{
    public interface IBookingService
    {
        Task<string> CreateReservation(BookingRequestModel model);
    }
}
