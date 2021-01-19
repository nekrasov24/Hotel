using BookingService.BookingService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingService.Quartz.Job
{
    [DisallowConcurrentExecution]
    public class Job : IJob
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ILogger<Job> _logger;
        private readonly IServiceProvider _pr;
        private readonly IBookingService _bookingService;


        public Job(IServiceScopeFactory serviceScopeFactory, ILogger<Job> logger, IServiceProvider pr, IBookingService bookingService)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _pr = pr;
            _bookingService = bookingService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("__________TRA TRA TRA TRA_______________");

            await _bookingService.CheckReservation();

        }
    }
}
