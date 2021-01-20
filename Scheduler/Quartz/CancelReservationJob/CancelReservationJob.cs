using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Scheduler.Publisher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheduler.Quartz.CancelReservationJob
{
    [DisallowConcurrentExecution]
    public class CancelReservationJob : IJob
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ILogger<CancelReservationJob> _logger;
        private readonly IServiceProvider _pr;
        private readonly IPublisher _publisher;


        public CancelReservationJob(IServiceScopeFactory serviceScopeFactory, ILogger<CancelReservationJob> logger, IServiceProvider pr,
            IPublisher publisher)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _pr = pr;
            _publisher = publisher;
        }
        
        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("__________Let's check_______________");

            var mesage = new JobMessage() { Message = "Let's check"};

            await _publisher.PublishJobMessage(mesage);

        }
    }
}
