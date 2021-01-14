using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RoomService.RoomService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomService.Subscriber
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

        public void Subscribe()
        {
            var queue = _bus.QueueDeclare("Reservation");

            _bus.Consume<string>(queue, async (msg, info) => {
                var roomService = _pr.GetRequiredService<IRoomService>();

                await roomService.ChangeStatus(DeserializeTest(msg.Body));
            });



        }

        private TransferReservation DeserializeTest(string bodyMessage)
        {
            var message = JsonConvert.DeserializeObject<TransferReservation>(bodyMessage);
            return message;
        }
    }
}
