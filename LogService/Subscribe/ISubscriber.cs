using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogService.Subscribe
{
    public interface ISubscriber
    {
        void Subscribe();
    }
}
