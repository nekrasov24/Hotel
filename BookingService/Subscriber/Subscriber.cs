using BookingService.BookingService;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
        private readonly IBus _rpcBus;
        private readonly IServiceProvider _pr;
        private readonly ILogger _logger;

        public Subscriber(IAdvancedBus bus, IServiceProvider pr, IBus rpcBus, ILogger<Subscriber> logger)
        {
            _bus = bus;
            _pr = pr;
            _rpcBus = rpcBus;
            _logger = logger;

        }



        public void SubscribeJobMessage()
        {
            var queue = _bus.QueueDeclare("JobMessage");

            _bus.Consume<string>(queue, async (msg, info) =>
            {
                using var serviceScope = _pr.GetRequiredService<IServiceScopeFactory>().CreateScope();
                var bookingService = serviceScope.ServiceProvider.GetService<IBookingService>();
                await bookingService.CheckReservation(DeserializeJobMessage(msg.Body));
            });
        }

        public async Task SubscribeVerificationReservationId()
        {
            await _rpcBus.Rpc.RespondAsync<string, string>(async (responseString, token) => await DeserializeVerify(responseString),
                c => c.WithQueueName(nameof(VerificationReservationId)));
        }




        private JobMessage DeserializeJobMessage(string bodyMessage)
        {
            var message = JsonConvert.DeserializeObject<JobMessage>(bodyMessage);
            return message;
        }

        private async Task<string> DeserializeVerify(string bodyMessage)
        {
            var message = JsonConvert.DeserializeObject<VerificationReservationId>(bodyMessage);
            using var serviceScope = _pr.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var bookingService = serviceScope.ServiceProvider.GetService<IBookingService>();
            var mes = bookingService.VerifyReservationId(message);

            _logger.LogWarning($"logger readed {message}");

            return mes.ToString();
        }


    }
}
