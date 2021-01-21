using Newtonsoft.Json;
using OrderService.HeaderService;
using OrderService.Model;
using OrderService.Publisher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IHeaderService _headerService;
        private readonly IPublisher _publisher;

        public OrderService( IHeaderService headerService)
        {
            _headerService = headerService;
        }

        public async Task<string> PayRoom(OrderRequestModel model)
        {
            try
            {
                var userId = _headerService.GetUserId();
                var verify = new VerificationReservationId() 
                {
                    ReservationId = model.ReservationId.ToString(),
                    UserId = userId.ToString() 
                };

                var reservation = await _publisher.VerifyReservationId(verify);

                var receiveReservation = JsonConvert.DeserializeObject<ReceiveReservation>(reservation);

                var amountPaid = receiveReservation.AmountPaid.ToString();

                var pay = new PaymentModel() { AmountPaid = amountPaid, UserId = userId.ToString() };

                var message = await _publisher.PayRoom(pay);
                var errorMessage = "I'm sorry, but it appears your account has insufficient funds";

                if (message == errorMessage)
                {
                    throw new Exception(message);
                }


                var dateOfPayment = DateTime.UtcNow;
                var newOrder = new Order()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    RoomId = receiveReservation.RoomId,
                    ReservStartDate = receiveReservation.ReservStartDate,
                    ReservFinishDate = receiveReservation.ReservFinishedDate,
                    DateOfPayment = dateOfPayment,
                    AmountPaid = receiveReservation.AmountPaid,
                };




                return message;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
