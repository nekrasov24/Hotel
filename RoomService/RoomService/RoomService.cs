
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RoomService.FileService;
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
        private readonly IRepository<Room, Guid> _roomRepository;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IRepository<RoomImage, Guid> _imagesRepository;

        public RoomService(IRepository<Room, Guid> roomRepository, IMapper mapper, IFileService fileService,
            IRepository<RoomImage, Guid> imagesRepository)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
            _fileService = fileService;
            _imagesRepository = imagesRepository;
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
                var file = model.Images;
                var roomId = newRoom.Id;
                var image = _fileService.AddImageToDbAsync(file, roomId);

                await _imagesRepository.AddImageAsync(image);

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
                var room = (await _roomRepository.GetAllAsync(r => r.Id == model.Id)).FirstOrDefault();

                if (room == null) throw new Exception("Room doesn't exists");


                _mapper.Map(model, room);

                await _roomRepository.EditRoom(room);


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

        public async Task<IEnumerable<RoomDTO>> GetAllRoomsAsync()
        {

            var getRooms = await (await _roomRepository.GetAllAsync(includes: (r) => r.Include(i => i.RoomImages))).ToArrayAsync();


            var roomModel = _mapper.Map<IEnumerable<RoomDTO>>(getRooms);

            foreach (var item in roomModel)
            {
                foreach (var imageModel in item.RoomImages)
                {
                    imageModel.ImagePath = await _fileService.GetAllImageAsync(imageModel.ImagePath);
                }
            }

            return roomModel;
        }
        public Room GetRoom(Guid id)
        {
            var room = _roomRepository.GetRoomById(id);
            if (room == null) throw new Exception($"Room was not found");

            return room;
        }

        

















        //return await(await _roomRepository.GetAllAsync(includes: (r) => r.Include(i => i.RoomImages))).ToListAsync();
    }
}
