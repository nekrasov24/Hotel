using AutoMapper;
using RoomService.RoomModel;
using RoomService.RoomService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomService.MapperProfile
{
    public class RoomProfiles : Profile
    {
        public RoomProfiles()
        {
            CreateMap<EditRoomRequestModel, Room>();
        }
        
    }
}
