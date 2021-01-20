using EasyNetQ;
using EasyNetQ.Topology;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RoomService.Subscriber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomService.Publisher
{
    public class Publisher : IPublisher
    {
        private readonly IAdvancedBus _bus;

        public Publisher(IAdvancedBus bus)
        {
            _bus = bus;
        }

        public async Task Publish(string mes)
        {
            var queueExhange = nameof(PublishMessage);
            var queue2 = _bus.QueueDeclare(queueExhange);
            var exchange = _bus.ExchangeDeclare(queueExhange, ExchangeType.Topic);
            _bus.Bind(exchange, queue2, "A.*");
            var newMessage = new PublishMessage() { Text = "Hello"};
            var topic = $"ProjectId.CabinId";
            var yourMessage = new Message<string>(JsonConvert.SerializeObject(new PublishMessage { Text = mes }));
            await  _bus.PublishAsync(exchange, "A.*", true, yourMessage);

        }

        


    }
}
