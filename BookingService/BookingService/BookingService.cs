using AutoMapper;
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
        private readonly IMapper _mapper;

        public BookingService(IReservationSettings settings, IHeaderService headerService, IPublisher publicher, IMapper mapper)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _reservation = database.GetCollection<Reservation>(settings.ReservationCollectionName);
            _headerService = headerService;
            _publicher = publicher;
            _mapper = mapper;
        }
        public async Task<string> CreateReservation(BookingRequestModel model)
        {
            try
            {
                var filter = Builders<Reservation>.Filter.Eq("RoomId", model.RoomId);
                var reservation = _reservation.Find(filter).FirstOrDefault();
                if (reservation != null) throw new Exception("Room can't be booked");

                var startDate = DateTime.UtcNow;
                var finishDate = startDate.AddMinutes(2);
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
                    RoomId = newReservation.RoomId
                };
                await _publicher.Publish(newTransferReservation);
                return "Reservation was added successfully";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> CancelReservation(Guid id)
        {
            try
            {
                var filter = Builders<Reservation>.Filter.Eq("Id", id);
                var reservation = _reservation.Find(filter).FirstOrDefault();
                if(reservation == null) throw new Exception("Book doesn't existst");

                var newTransferReservation = new CancelReservation()
                {
                    RoomId = reservation.RoomId
                };

                await _publicher.CancelPublish(newTransferReservation);


                _reservation.DeleteOne(Builders<Reservation>.Filter.Eq("Id", id));

                return "Reservation was canceled successfully";
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public IEnumerable<ReservationDTO> GetReservation()
        {
            try
            {
                var userId = _headerService.GetUserId();

                var filter = Builders<Reservation>.Filter.Eq("UserId", userId);
                var reservation = _reservation.Find(filter).ToList();
                if (reservation == null) throw new Exception("Book doesn't existst");
                var modelReservation = _mapper.Map<IEnumerable<ReservationDTO>>(reservation);

                return modelReservation;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }        
    }
}
