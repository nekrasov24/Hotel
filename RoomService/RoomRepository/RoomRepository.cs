using RoomService.RoomModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomService.RoomRepository
{
    public class RoomRepository : IRoomRepository
    {
        private readonly RoomContext _roomContext;

        public RoomRepository(RoomContext roomContext)
        {
            _roomContext = roomContext;
        }
    }
}
