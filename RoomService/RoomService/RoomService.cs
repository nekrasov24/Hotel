
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RoomService.FileService;
using RoomService.Publisher;
using RoomService.RoomModel;
using RoomService.RoomRepository;
using RoomService.Subscriber;
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
        private readonly IPublisher _publisher;

        public RoomService(IRepository<Room, Guid> roomRepository, IMapper mapper, IFileService fileService,
            IRepository<RoomImage, Guid> imagesRepository, IPublisher publisher)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
            _fileService = fileService;
            _imagesRepository = imagesRepository;
            _publisher = publisher;
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
                    RoomType = model.RoomType,
                    Status = Status.Free
                    
                };

                await _roomRepository.AddRoomAsync(newRoom);
                var file = model.Images;
                var roomId = newRoom.Id;
                foreach(var image in model.Images)
                {
                    var roomImage = _fileService.WriteImage(image, roomId);

                    await _imagesRepository.AddImageAsync(roomImage);
                }
                var response = $"Number id: {newRoom.Id}: {newRoom.Name} was added successfully";
                await _publisher.Publish(response);
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
                var room =  _roomRepository.GetRoomById(model.Id);
                if (room == null) throw new Exception("Room doesn't exists");

                if(model.ListImageId?.Count > 0)
                {
                    foreach (var imageId in model.ListImageId)
                    {
                        var deleteImage = (await _imagesRepository.GetAllAsync(i => i.Id == imageId)).FirstOrDefault();
                        if (deleteImage == null) throw new Exception("Images don't exists");
                        _fileService.DeleteImage(deleteImage.ImagePath);
                        await _imagesRepository.DeleteImageAsync(deleteImage);
                    }
                }
                

                _mapper.Map(model, room);
                await _roomRepository.EditRoom(room);

                if (model.Images?.Count > 0)
                {
                    foreach (var image in model.Images)
                    {
                        var roomImage = _fileService.WriteImage(image, model.Id);
                        await _imagesRepository.AddImageAsync(roomImage);
                    }
                }
                

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
                var find = _roomRepository.GetRoomById(id);
                if(find == null) throw new Exception("Room doesn't exists");

                if (find.RoomImages != null)
                {
                    foreach (var image in find.RoomImages)
                    {
                        _fileService.DeleteImage(image.ImagePath);
                    }
                }

                await _roomRepository.DeleteRoom(find);
                
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


            return roomModel;
        }
        public async Task<RoomDTO> GetRoom(Guid id)
        {
            var room = _roomRepository.GetRoomById(id);
            if (room == null) throw new Exception($"Room was not found");

            var roomModel = _mapper.Map<RoomDTO>(room);

            return roomModel;
        }

        public async Task ChangeStatus(TransferReservation reservation)
        {
            var room = _roomRepository.GetRoomById(reservation.RoomId);
            room.Status = Status.Booked;      
            await _roomRepository.EditRoom(room);
        }

        public async Task ChangeStatusToFree(CancelReservation reservation)
        {
            var room = _roomRepository.GetRoomById(reservation.RoomId);
            room.Status = Status.Free;
            await _roomRepository.EditRoom(room);
        }

    }
}
