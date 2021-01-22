using Authorization.UserService;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.Subscriber
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

        public async Task SubscribePayRoom()
        {
            await _rpcBus.Rpc.RespondAsync<string, string>(async (responseString, token) => await DeserializeVerify(responseString),
                c => c.WithQueueName(nameof(PaymentModel)));
        }

        private async Task<string> DeserializeVerify(string bodyMessage)
        {
            try
            {
                _logger.LogInformation("DeserializeVerify {0}", bodyMessage);

                var message = JsonConvert.DeserializeObject<PaymentModel>(bodyMessage);


                using var serviceScope = _pr.GetRequiredService<IServiceScopeFactory>().CreateScope();
                var userService = serviceScope.ServiceProvider.GetService<IUserService>();
                var mes = await userService.PayRoom(message);

                _logger.LogWarning($"logger readed {message}");

                return mes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail Subscribe");
                throw;
            }
        }
    }
}
