using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.Publisher
{
    public interface IPublisher
    {
        Task Publish(string mes);
    }
}
