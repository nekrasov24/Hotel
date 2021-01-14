using BookingService.HeaderService;
using BookingService.Model;
using BookingService.Publisher;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingService.BookingService
{
    public class BookingService : IBookingService
    {
        private readonly IMongoCollection<Reservation> _reservation;
        private readonly IHeaderService _headerService;
        private readonly IPublisher _publicher;

        public BookingService(IReservationSettings settings, IHeaderService headerService, IPublisher publicher)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _reservation = database.GetCollection<Reservation>(settings.ReservationCollectionName);
            _headerService = headerService;
            _publicher = publicher;
        }
        public async Task<string> CreateReservation(BookingRequestModel model)
        {
            var startDate = DateTime.UtcNow;
            var finishDate = startDate.AddMinutes(56);

            var userId = _headerService.GetUserId();
            var newReservation = new Reservation()
            {
                Id = Guid.NewGuid(),
                RoomId = model.RoomId,
                UserId = userId,
                StartDateOfBooking = startDate,
                FinishDateOfBooking = finishDate,
                ReservStartDate = model.ReservStartDate,
                ReservFinishedDate = model.ReservFinishedDate
            };

            _reservation.InsertOne(newReservation);


            var newTransferReservation = new TransferReservation()
            {
                Id =newReservation.Id,
                RoomId = newReservation.RoomId,
                UserId = newReservation.UserId,
                StartDateOfBooking = newReservation.StartDateOfBooking,
                FinishDateOfBooking = newReservation.StartDateOfBooking,
                ReservStartDate = newReservation.ReservStartDate,
                ReservFinishedDate = newReservation.ReservFinishedDate
            };

            await _publicher.Publish(newTransferReservation);
            
            return "";
        }

        
    }
}
