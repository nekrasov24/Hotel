using RoomService.Subscriber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomService.Publisher
{
    public interface IPublisher
    {
        Task Publish(string mes);

    }
}
