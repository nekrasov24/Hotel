using RoomService.FileService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomService.ImagesService
{
    public interface IImagesService
    {
        Task<string> AddImageAsync(AddImageRequestModel imageRequest);
        Task<string> DeleteImageAsync(DeleteImageRequestModel imageRequest);
    }
}
