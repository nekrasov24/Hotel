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
        Task<string> GetAllImageAsync(string filePath);
        RoomImage AddImageToDbAsync(IFormFile imageRequest, Guid id);
    }
}

