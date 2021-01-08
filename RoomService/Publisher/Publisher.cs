using EasyNetQ;
using EasyNetQ.Topology;
using Microsoft.Extensions.Configuration;
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

        public async Task Publish()
        {
            //string message = "hello";

            var queue = _bus.QueueDeclare("MyQueue");
            var newMessage = new PublishMessage() { Text = "Hello"};
            await _bus.PublishAsync(Exchange.GetDefault(), queue.Name, true, new Message<PublishMessage>(newMessage));
            //ConsumeMessage(_bus);
            //await _bus.PubSub.PublishAsync(JsonConvert.SerializeObject(new MessageA { Text = "Hello World" }), "asd");
            //await _bus.PubSub.PublishAsync(newMessage, "asd");
            //_bus.PubSub.Subscribe<PublishMessage>("my_subscription_id", msg => Console.WriteLine(msg.Text));
        }




    }
}
