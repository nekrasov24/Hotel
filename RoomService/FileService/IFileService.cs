using Microsoft.AspNetCore.Http;
using RoomService.RoomModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomService.FileService
{
    public interface IFileService
    {
        RoomImage WriteImage(IFormFile imageRequest, Guid id);
        void DeleteImage(string filePath);
    }
}

