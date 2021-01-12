using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ;

namespace RoomService.Publisher
{
    public class PublishMessage
    {
        public string Text { get; set; }
    }
}
