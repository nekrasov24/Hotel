using AutoMapper;
using BookingService.BookingService;
using BookingService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingService.MapperProfile
{
    public class ReservProfiles : Profile
    {
        public ReservProfiles()
        {
            CreateMap<Reservation, TransferReservation>();

        }
    }
}
