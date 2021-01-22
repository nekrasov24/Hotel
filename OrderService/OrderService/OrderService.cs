using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OrderService.HeaderService;
using OrderService.Model;
using OrderService.OrderRepository;
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
        private readonly IOrderRepository<Order, Guid> _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;

        public OrderService( IHeaderService headerService, IPublisher publisher, IOrderRepository<Order, Guid> orderRepository, IMapper mapper,
            ILogger<OrderService> logger)
        {
            _headerService = headerService;
            _publisher = publisher;
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> PayRoom(OrderRequestModel model)
        {
            try
            {
                if(model.ReservationId == null) throw new Exception("operation is impossible without Id");
                var userId = _headerService.GetUserId();
                if(userId == null) throw new Exception("operation is impossible without Id");
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

                var m = "spspspsps";
                var mmm = "rtrtrtrtrtrtr";


                if (message == m)
                {
                    throw new Exception(message);
                }

                if (message == mmm)
                {
                    throw new Exception(message);
                }

                var errorMessage = "I'm sorry, but it appears your account has insufficient funds";

                if (message == errorMessage)
                {
                    throw new Exception(message);
                }

                //var dateOfPayment = DateTime.UtcNow;
                //var newOrder = new Order()
                //{
                //    Id = Guid.NewGuid(),
                //    UserId = userId,
                //    RoomId = receiveReservation.RoomId,
                //    ReservStartDate = receiveReservation.ReservStartDate,
                //    ReservFinishDate = receiveReservation.ReservFinishedDate,
                //    DateOfPayment = dateOfPayment,
                //    AmountPaid = receiveReservation.AmountPaid,
                //};

                //await _orderRepository.AddOrderAsync(newOrder);

                return message;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "faild orderService");
                throw;
                
            }
        }

        public async Task<IEnumerable<OrderDTO>> GetAllOrders()
        {
            var userId = _headerService.GetUserId();
            var getOrders = await (await _orderRepository.GetAllAsync(predicate: (o) => o.UserId.Equals(userId))).ToArrayAsync();
            
            var getOrdersDTO = _mapper.Map<IEnumerable<OrderDTO>>(getOrders);
            return getOrdersDTO;

        }
    }
}
