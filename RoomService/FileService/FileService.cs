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




        public RoomImage WriteImage(IFormFile imageRequest, Guid roomId)
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
            //image.Mutate(t => t.Resize(new ResizeOptions { Mode = ResizeMode.Max, Size = new Size(280, 280) }));
            //image.Mutate(t => t.Resize(280,176));
            image.SaveAsJpeg(outputStream);
            int length = (int)outputStream.Length;
            byte[] bytes = new byte[length];
            outputStream.Read(bytes, 0, length);

            var newImage = new RoomImage()
            {
                Id = imageId,
                ImagePath = imagePath,
                RoomId = roomId,
            };

            return newImage;

        }
       
        public async Task<string> GetAllImageAsync(string filePath)
        {
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open))
                {
                    int length = (int)fileStream.Length;
                    byte[] bytes = new byte[length];
                    await fileStream.ReadAsync(bytes, 0, length);
                    return Convert.ToBase64String(bytes);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void DeleteImage(string filePath)
        {
            File.Delete(filePath);
        }





    }
}
