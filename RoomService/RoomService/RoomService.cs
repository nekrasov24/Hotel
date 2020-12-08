
using AutoMapper;
using RoomService.RoomModel;
using RoomService.RoomRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomService.RoomService
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public RoomService(IRoomRepository roomRepository, IMapper mapper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        public async Task<string> AddARoomAsync(RoomRequestModel model)
        {
            try
            {
                var room = (await _roomRepository.GetAllAsync(r => r.Number.Equals(model.Number))).FirstOrDefault();
                if (room != null) throw new Exception("Room already exists");
                var newRoom = new Room
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    Number = model.Number,
                    NumberOfPeople = model.NumberOfPeople,
                    PriceForNight = model.PriceForNight,
                    Description = model.Description,
                    RoomType = model.RoomType
                };
                await _roomRepository.AddRoomAsync(newRoom);
                return "Number was added successfully";
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> EditRoomAsync(EditRoomRequestModel model)
        {
            try
            {
                var updateRoom = _mapper.Map<Room>(model);

                if (updateRoom == null) throw new Exception("Room doesn't exists");

                await _roomRepository.EditRoom(updateRoom);
                return "Number was edited successfully";
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

        public async Task<string> DeleteRoomAsync(Guid id)
        {
            try
            {
                if (id == null) throw new Exception("Request is incorrect");
                var find = await _roomRepository.FindRoomAsync(id);
                if(find == null) throw new Exception("Room doesn't exists");
                await _roomRepository.DeleteRoom(id);
                
                return "Number was deleted successfully";
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
