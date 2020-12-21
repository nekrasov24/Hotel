using Microsoft.AspNetCore.Hosting;
using SixLabors.ImageSharp;
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

        [Obsolete]
        public FileService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public string SetPath(ImageRequest imageRequest)
        {

            var imageName = imageRequest.Id.ToString() + ".jpeg";
            var imageSetPath = Path.Combine(_hostingEnvironment.ContentRootPath, @"files/");
            var imagePath = imageSetPath + imageName;

            var directory = new DirectoryInfo(imageSetPath);
            if (!directory.Exists)
            {
                directory.Create();
            }

            using var image = Image.Load(imageRequest.Image.OpenReadStream());
            using var outputStream = new FileStream(imagePath, FileMode.Create);
            image.SaveAsGif(outputStream);
            int length = (int)outputStream.Length;
            byte[] bytes = new byte[length];
            outputStream.Read(bytes, 0, length);

            return imagePath;
        }

       /*ublic string GetImageAsBase64()
        {
            using (var fileStream = new FileStream(filepath, FileMode.Open))
            {
                int length = (int)fileStream.Length;
                byte[] bytes = new byte[length];
                fileStream.Read(bytes, 0, length);
                return Convert.ToBase64String(bytes);
            }
        }*/
    }
}
