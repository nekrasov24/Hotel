using BookingService.BookingService;
using BookingService.Model;
using EasyNetQ;
using EasyNetQ.Topology;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingService.Publisher
{
    public class Publisher : IPublisher
    {
        private readonly IAdvancedBus _bus;
        private readonly IBus _rpcBus;
        private readonly ILogger _logger;

        public Publisher(IAdvancedBus bus, IBus rpcBus, ILogger<Publisher> logger)
        {
            _bus = bus;
            _rpcBus = rpcBus;
            _logger = logger;
        }

        public async Task Publish(TransferReservation reservation)
        {
            var queueExhange = nameof(TransferReservation);
            var queue2 = _bus.QueueDeclare(queueExhange);
            var exchange = _bus.ExchangeDeclare(queueExhange, ExchangeType.Topic);
            _bus.Bind(exchange, queue2, "A.*");
            var topic = $"ProjectId.CabinId";
            var yourMessage = new Message<string>(JsonConvert.SerializeObject(reservation));
            await _bus.PublishAsync(exchange, "A.*", true, yourMessage);
        }

        public async Task CancelPublish(CancelReservation reservation)
        {
            var queueExhange = nameof(CancelReservation);
            var queue2 = _bus.QueueDeclare(queueExhange);
            var exchange = _bus.ExchangeDeclare(queueExhange, ExchangeType.Topic);
            _bus.Bind(exchange, queue2, "A.*");

            var topic = $"ProjectId.CabinId";
            var yourMessage = new Message<string>(JsonConvert.SerializeObject(reservation));
            await _bus.PublishAsync(exchange, "A.*", true, yourMessage);

        }

        public async Task<string> VerifyRoomId(VerificationRoomId verificationtion)
        {
            var response = await _rpcBus.Rpc.RequestAsync<string, string>(JsonConvert.SerializeObject(verificationtion), 
                c => c.WithQueueName(nameof(VerificationRoomId)));

            _logger.LogWarning($"logger readed {response}");

            return response;

        }

        /*public async Task<string> GetPriceForNight(Price mes)
        {
            var response = await _rpcBus.Rpc.RequestAsync<string, string>(JsonConvert.SerializeObject(mes),
                c => c.WithQueueName(nameof(Price)));

            _logger.LogWarning($"logger readed {response}");

            return response;

        }*/
    }
}
