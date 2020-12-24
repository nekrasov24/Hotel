using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomService.FileService
{
    public class ImageRequest
    {
        public Guid RoomId { get; set; }
        public IFormFile Image { get; set; }
    }
}
