using EasyNetQ;
using EasyNetQ.Consumer;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogService.Subscribe
{
    public class Subscriber : ISubscriber
    {
        private readonly IAdvancedBus _bus;
        private readonly ILogger _logger;

        public Subscriber(IAdvancedBus bus, ILogger<Subscriber> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        public void Subscribe()
        {
            var queue = _bus.QueueDeclare("MyQueue");
            _bus.Consume(queue, registration =>
            {
                registration.Add<PublishMessage>((message, info) => { Console.WriteLine("Body: {0}", message.Body); });
            });
            _logger.LogInformation("Rammstein");
            //_bus.PubSub.Subscribe<PublishMessage>("my_subscription_id", msg => Console.WriteLine(msg.Text), new Action<ISubscriptionConfiguration>(o => o.WithTopic("asd")));
        }
    }     
}
