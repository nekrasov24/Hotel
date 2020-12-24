using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using RoomService.RoomModel;
using RoomService.RoomRepository;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RoomService.FileService
{
    public class FileService : IFileService
    {
        
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IRepository<RoomImage, Guid> _imagesRepository;
        private readonly IRepository<Room, Guid> _roomRepository;

        [Obsolete]
        public FileService(IHostingEnvironment hostingEnvironment, IRepository<RoomImage, Guid> imagesRepository,
            IRepository<Room, Guid> roomRepository)
        {
            _hostingEnvironment = hostingEnvironment;
            _imagesRepository = imagesRepository;
            _roomRepository = roomRepository;
        }




        public RoomImage AddImageAsync(IFormFile imageRequest, Guid roomId)
        {
            var imageId = Guid.NewGuid();
            var imageName = imageId.ToString() + ".jpeg";
            var imageSetPath = Path.Combine(_hostingEnvironment.ContentRootPath, @"files/");
            var imagePath = imageSetPath + imageName;

            var directory = new DirectoryInfo(imageSetPath);
            if (!directory.Exists)
            {
                directory.Create();
            }

            using var image = Image.Load(imageRequest.OpenReadStream());
            using var outputStream = new FileStream(imagePath, FileMode.Create);
            image.Mutate(t => t.Resize(new ResizeOptions { Mode = ResizeMode.Max, Size = new Size(500, 500) }));
            image.SaveAsJpeg(outputStream);
            int length = (int)outputStream.Length;
            byte[] bytes = new byte[length];
            outputStream.Read(bytes, 0, length);

            //var roomId = id;

            var newImage = new RoomImage()
            {
                Id = imageId,
                ImagePath = imagePath,
                RoomId = roomId,
            };

            return newImage;

        }

        public RoomImage EditImageAsync(IFormFile imageRequest, Guid id)
        {
            var imageId = Guid.NewGuid();
            var imageName = id.ToString() + ".jpeg";
            var imageSetPath = Path.Combine(_hostingEnvironment.ContentRootPath, @"files/");
            var imagePath = imageSetPath + imageName;

            var directory = new DirectoryInfo(imageSetPath);
            if (!directory.Exists)
            {
                directory.Create();
            }

            using var image = Image.Load(imageRequest.OpenReadStream());
            using var outputStream = new FileStream(imagePath, FileMode.Create);
            image.Mutate(t => t.Resize(new ResizeOptions { Mode = ResizeMode.Max, Size = new Size(500, 500) }));
            image.SaveAsJpeg(outputStream);
            int length = (int)outputStream.Length;
            byte[] bytes = new byte[length];
            outputStream.Read(bytes, 0, length);

            var newImage = new RoomImage()
            {
                Id = imageId,
                ImagePath = imagePath,
                RoomId = id,
            };

            return newImage;

        }




        public async Task<string> GetAllImageAsync(string filePath)
        {
            try
            {
                /*if (id == null) throw new Exception("Request is incorrect");
                var findImage = (await _imagesRepository.GetAllAsync(image => image.RoomId == id)).FirstOrDefault();
                if (findImage == null) throw new Exception("Image doesn't exists");
                var filePath = findImage.ImagePath;*/

                using (var fileStream = new FileStream(filePath, FileMode.Open))
                {
                    int length = (int)fileStream.Length;
                    byte[] bytes = new byte[length];
                    fileStream.Read(bytes, 0, length);
                    return Convert.ToBase64String(bytes);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





    }
}
