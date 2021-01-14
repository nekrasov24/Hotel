using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingService.BookingService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost("book")]
        public async Task<IActionResult> CreateReservation(BookingRequestModel model)
        {
            var response = await _bookingService.CreateReservation(model);
            return Ok(response);
        }
    }
}
