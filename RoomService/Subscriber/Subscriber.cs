using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

        public void Subscribe()
        {
            var queue = _bus.QueueDeclare("TransferReservation");

            _bus.Consume<string>(queue, async(msg, info) => {
                using var serviceScope = _pr.GetRequiredService<IServiceScopeFactory>().CreateScope();
                var roomService = serviceScope.ServiceProvider.GetService<IRoomService>();
                await roomService.ChangeStatus(DeserializeTest(msg.Body));
            });
        }

        public async Task SubscribeVerificationRoomId()
        {
           await _rpcBus.Rpc.RespondAsync<string, string>(async (responseString, token) => await DeserializeVerify(responseString), 
               c => c.WithQueueName(nameof(VerificationRoomId)));
        }


        public void SubscribeCancel()
        {
            var queue = _bus.QueueDeclare("CancelReservation");

            _bus.Consume<string>(queue, async (msg, info) =>
            {
                using var serviceScope = _pr.GetRequiredService<IServiceScopeFactory>().CreateScope();
                var roomService = serviceScope.ServiceProvider.GetService<IRoomService>();
                await roomService.ChangeStatusToFree(DeserializeCancel(msg.Body));
            });
        }


        private TransferReservation DeserializeTest(string bodyMessage)
        {
            var message = JsonConvert.DeserializeObject<TransferReservation>(bodyMessage);
            return message;
        }

        private CancelReservation DeserializeCancel(string bodyMessage)
        {
            var message = JsonConvert.DeserializeObject<CancelReservation>(bodyMessage);
            return message;
        }

        private async Task<string> DeserializeVerify(string bodyMessage)
        {
            var message = JsonConvert.DeserializeObject<VerificationRoomId>(bodyMessage);
            var mess = message.RoomId;
            using var serviceScope = _pr.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var roomService = serviceScope.ServiceProvider.GetService<IRoomService>();
            var mes = roomService.VerifyRoomId(mess);

            _logger.LogWarning($"logger readed {message}");

            return mes.ToString();
        }
    }
}
