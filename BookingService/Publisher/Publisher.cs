using BookingService.BookingService;
using BookingService.Model;
using EasyNetQ;
using EasyNetQ.Topology;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingService.Publisher
{
    public class Publisher : IPublisher
    {
        private readonly IAdvancedBus _bus;
        private readonly IBus _rpcBus;
        private readonly ILogger _logger;

        public Publisher(IAdvancedBus bus, IBus rpcBus, ILogger<Publisher> logger)
        {
            _bus = bus;
            _rpcBus = rpcBus;
            _logger = logger;
        }



        public async Task<string> VerifyRoomId(VerificationRoomId verificationtion)
        {
            var response = await _rpcBus.Rpc.RequestAsync<string, string>(JsonConvert.SerializeObject(verificationtion), 
                c => c.WithQueueName(nameof(VerificationRoomId)));

            _logger.LogWarning($"logger readed {response}");

            return response;

        }

    }
}
