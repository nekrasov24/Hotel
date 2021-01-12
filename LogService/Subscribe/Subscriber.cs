using EasyNetQ;
using EasyNetQ.Consumer;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
            var queue = _bus.QueueDeclare("PublishMessage");

            _bus.Consume<string>(queue, (msg, info) => DeserializeTest(msg.Body));

        }

        private void DeserializeTest(string bodyMessage)
        {
            var message = JsonConvert.DeserializeObject<PublishMessage>(bodyMessage);
            Console.WriteLine(message.Text);
            _logger.LogInformation(message.Text);
        }
    }     
}
