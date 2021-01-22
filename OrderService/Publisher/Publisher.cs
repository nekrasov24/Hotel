using EasyNetQ;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OrderService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Publisher
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

        public async Task<string> VerifyReservationId(VerificationReservationId verificationtion)
        {

            var response = await _rpcBus.Rpc.RequestAsync<string, string>(JsonConvert.SerializeObject(verificationtion),
                c => c.WithQueueName(nameof(VerificationReservationId)));


            return response;

        }

        public async Task<string> PayRoom(PaymentModel payment)
        {

           
            var response = await _rpcBus.Rpc.RequestAsync<string, string>(JsonConvert.SerializeObject(payment),
                c => c.WithQueueName(nameof(PaymentModel)));



            return response;

        }
    }
}
