
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
            catch
            {
                throw;
            }
        }

        public async Task<string> EditRoomAsync(UpdateRoomModelRequest model)
        {

            var updateRoom = _mapper.Map<Room>(model);

            await _roomRepository.UpdateRoom(updateRoom);
            return "";
        }

        public async Task<string> DeleteRoomAsync(Guid id)
        {
            await _roomRepository.DeleteRoomByNumber(id);
            return "Number was delited successfully";
        }
    }
}
