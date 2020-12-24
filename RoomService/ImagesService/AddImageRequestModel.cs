using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomService.ImagesService
{
    public class AddImageRequestModel
    {
        public Guid RoomId { get; set; }
        public IFormCollection Images { get; set; }
    }
}
