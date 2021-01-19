using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheduler.Publisher
{
    public interface IPublisher
    {
        Task PublishJobMessage(JobMessage message);
    }
}
