using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogService.LogService
{
    public class LogService
    {
        private readonly ILogger _logger;

        public LogService(ILogger<LogService> logger)
        {
            _logger = logger;
        }


    }

}
