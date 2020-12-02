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
                return "";
            }
            catch
            {
                throw;
            }
        }

        public async Task<string> DeliteRoomAsync(string number)
        {
            await _roomRepository.DeliteRoomByNumber(number);
            return "";
        }
    }
}
