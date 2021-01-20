using AutoMapper;
using BookingService.HeaderService;
using BookingService.Model;
using BookingService.Publisher;
using BookingService.Subscriber;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<BookingService> _logger;

        public BookingService(IReservationSettings settings, IHeaderService headerService, IPublisher publicher, IMapper mapper,
            ILogger<BookingService> logger)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _reservation = database.GetCollection<Reservation>(settings.ReservationCollectionName);
            _headerService = headerService;
            _publicher = publicher;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<string> CreateReservation(BookingRequestModel model)
        {
            try
            {

                VerificationRoomId verify = new VerificationRoomId() { RoomId = model.RoomId.ToString() };

                var roomId = await _publicher.VerifyRoomId(verify);
                var message = "Ok";
                _logger.LogInformation(message);
                _logger.LogInformation(message);
                _logger.LogInformation(message);
                _logger.LogInformation(message);
                _logger.LogInformation(message);
                _logger.LogInformation(message);
                _logger.LogInformation(message);

                if (roomId == message)
                {
                    if (model.ReservFinishedDate <= model.ReservStartDate)
                        throw new Exception("Start date can not be biggest than finish date");

                    var filter = Builders<Reservation>.Filter.Eq("RoomId", model.RoomId);

                    var listReservations = _reservation.Find(filter).ToList();
                    //if (reservation != null) throw new Exception("Room can't be booked");

                    foreach (var reservation in listReservations)
                    {
                        if (model.ReservStartDate >= reservation.ReservStartDate &&
                            model.ReservStartDate <= reservation.ReservFinishedDate ||
                            model.ReservFinishedDate >= reservation.ReservStartDate &&
                            model.ReservFinishedDate <= reservation.ReservFinishedDate)
                        {
                            throw new Exception("Room can't be booked");
                        }
                        else
                        {
                            if (model.ReservStartDate <= reservation.ReservStartDate &&
                                model.ReservFinishedDate >= reservation.ReservFinishedDate)
                            {
                                throw new Exception("Room can't be booked");
                            }
                        }
                    }

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
                        ReservFinishedDate = model.ReservFinishedDate,
                        NumberOfNights = (model.ReservFinishedDate - model.ReservStartDate).Days
                    };

                    _reservation.InsertOne(newReservation);
                    var newTransferReservation = new TransferReservation()
                    {
                        RoomId = newReservation.RoomId
                    };

                    await _publicher.Publish(newTransferReservation);

                    return "Reservation was added successfully";
                }
                throw new Exception(message);






                //var startDate = DateTime.UtcNow;
                //var finishDate = startDate.AddMinutes(2);
                //var userId = _headerService.GetUserId();

                //var newReservation = new Reservation()
                //{
                //    Id = Guid.NewGuid(),
                //    RoomId = model.RoomId,
                //    UserId = userId,
                //    StartDateOfBooking = startDate,
                //    FinishDateOfBooking = finishDate,
                //    ReservStartDate = model.ReservStartDate,
                //    ReservFinishedDate = model.ReservFinishedDate,
                //    NumberOfNights = (model.ReservFinishedDate - model.ReservStartDate).Days
                //};

                //_reservation.InsertOne(newReservation);
                //var newTransferReservation = new TransferReservation()
                //{
                //    RoomId = newReservation.RoomId
                //};

                //await _publicher.Publish(newTransferReservation);

                //return "Reservation was added successfully";
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

        public async Task CheckReservation(JobMessage message)
        {
            _logger.LogInformation("start check");
            var awaitMessage = "Let's check";

            if (message.Message == awaitMessage)
            {
                var allReservation = _reservation.Find(_ => true).ToList();

                foreach (var finishDates in allReservation)
                {
                    if (DateTime.UtcNow > finishDates.FinishDateOfBooking)
                    {
                        var newTransferReservation = new CancelReservation()
                        {
                            RoomId = finishDates.RoomId
                        };
                        await _publicher.CancelPublish(newTransferReservation);

                        var id = finishDates.Id;
                        _reservation.DeleteOne(Builders<Reservation>.Filter.Eq("Id", id));
                    }
                }
            }

            _logger.LogInformation("finish check");


            //var allReservation = _reservation.Find(_ => true).ToList();

            //foreach (var finishDates in allReservation)
            //{
            //    if (DateTime.UtcNow > finishDates.FinishDateOfBooking)
            //    {
            //        var newTransferReservation = new CancelReservation()
            //        {
            //            RoomId = finishDates.RoomId
            //        };
            //        await _publicher.CancelPublish(newTransferReservation);

            //        var id = finishDates.Id;
            //        _reservation.DeleteOne(Builders<Reservation>.Filter.Eq("Id", id));
            //    }
            //}
        }

        
    }
}
