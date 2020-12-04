
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


        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
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
            var room = mapper.Map<FooDto>(foo);

            await _roomRepository.UpdateRoom(con);
            return "";
        }

        public async Task<string> DeleteRoomAsync(string number)
        {
            await _roomRepository.DeleteRoomByNumber(number);
            return "Number was delited successfully";
        }
    }
}
