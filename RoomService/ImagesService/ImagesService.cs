using RoomService.FileService;
using RoomService.RoomModel;
using RoomService.RoomRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomService.ImagesService
{
    public class ImagesService : IImagesService
    {
        private readonly IFileService _fileService;
        private readonly IRepository<RoomImage, Guid> _imagesRepository;

        public ImagesService(IFileService fileService, IRepository<RoomImage, Guid> imagesRepository)
        {
            _fileService = fileService;
            _imagesRepository = imagesRepository;
        }

        public async Task<string> AddImageAsync(AddImageRequestModel imageRequest)
        {
            var roomId = imageRequest.RoomId;
            var roomImages = new List<RoomImage>();

            foreach (var image in imageRequest.Images.Files)
            {
                var roomImage = _fileService.AddImageAsync(image, roomId);
                roomImages.Add(roomImage);               
            }
            await _imagesRepository.AddRangeImagesAsync(roomImages);

            return "Images were added successfully";
        }

        public async Task<string> DeleteImageAsync(DeleteImageRequestModel imageRequest)
        {
            var roomId = imageRequest.RoomId;
            var roomImages = new List<RoomImage>();

            foreach (var image in imageRequest.Images.Files)
            {
                var roomImage = _fileService.AddImageAsync(image, roomId);
                roomImages.Add(roomImage);
            }
            await _imagesRepository.DeleteImageAsync(roomImages);

            return "Images were added successfully";
        }
    }
}
