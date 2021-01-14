using BookingService.BookingService;
using BookingService.Model;
using EasyNetQ;
using EasyNetQ.Topology;
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

        public Publisher(IAdvancedBus bus)
        {
            _bus = bus;
        }

        public async Task Publish(TransferReservation reservation)
        {
            var queueExhange = nameof(Reservation);
            var queue2 = _bus.QueueDeclare(queueExhange);
            var exchange = _bus.ExchangeDeclare(queueExhange, ExchangeType.Topic);
            _bus.Bind(exchange, queue2, "A.*");

            var topic = $"ProjectId.CabinId";
            var yourMessage = new Message<string>(JsonConvert.SerializeObject(reservation));
            await _bus.PublishAsync(exchange, "A.*", true, yourMessage);

        }
    }
}
