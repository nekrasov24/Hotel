using EasyNetQ;
using EasyNetQ.Topology;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheduler.Publisher
{
    public class Publisher : IPublisher
    {
        private readonly IAdvancedBus _bus;

        public Publisher(IAdvancedBus bus)
        {
            _bus = bus;
        }

        public async Task PublishJobMessage(JobMessage message)
        {
            var queueExhange = nameof(JobMessage);
            var queue2 = _bus.QueueDeclare(queueExhange);
            var exchange = _bus.ExchangeDeclare(queueExhange, ExchangeType.Topic);
            _bus.Bind(exchange, queue2, "A.*");

            var topic = $"ProjectId.CabinId";
            var yourMessage = new Message<string>(JsonConvert.SerializeObject(message));
            await _bus.PublishAsync(exchange, "A.*", true, yourMessage);

        }
    }
}
