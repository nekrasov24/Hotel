using EasyNetQ;
using EasyNetQ.Consumer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogService.Subscribe
{
    public class Subscriber : ISubscriber
    {
        private readonly IAdvancedBus _bus;

        public Subscriber(IAdvancedBus bus)
        {
            _bus = bus;
        }

        public void Subscribe()
        {
            var queue = _bus.QueueDeclare("MyQueue");
            _bus.Consume(queue, registration =>
            {
                registration.Add<PublishMessage>((message, info) => { Console.WriteLine("Body: {0}", message.Body); });
            });

            //_bus.PubSub.Subscribe<PublishMessage>("my_subscription_id", msg => Console.WriteLine(msg.Text), new Action<ISubscriptionConfiguration>(o => o.WithTopic("asd")));
        }
    }     
}
