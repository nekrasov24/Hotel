using BookingService.BookingService;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingService.Subscriber
{
    public class Subscriber : ISubscriber
    {
        private readonly IAdvancedBus _bus;

        private readonly IServiceProvider _pr;

        public Subscriber(IAdvancedBus bus, IServiceProvider pr)
        {
            _bus = bus;
            _pr = pr;
        }



        public void SubscribeJobMessage()
        {
            var queue = _bus.QueueDeclare("JobMessage");

            _bus.Consume<string>(queue, async (msg, info) =>
            {
                using var serviceScope = _pr.GetRequiredService<IServiceScopeFactory>().CreateScope();
                var roomService = serviceScope.ServiceProvider.GetService<IBookingService>();
                await roomService.CheckReservation(DeserializeJobMessage(msg.Body));
            });
        }

        private JobMessage DeserializeJobMessage(string bodyMessage)
        {
            var message = JsonConvert.DeserializeObject<JobMessage>(bodyMessage);
            return message;
        }


    }
}
