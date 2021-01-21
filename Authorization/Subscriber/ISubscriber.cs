using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.Subscriber
{
    public interface ISubscriber
    {
        Task SubscribePayRoom();
    }
}
