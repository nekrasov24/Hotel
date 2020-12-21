using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoomService.FileService;
using RoomService.RoomService;

namespace RoomService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public ImagesController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        public string SetImagePath(ImageRequest imageRequest)
        {
            var res = _roomService.SetImagePath(imageRequest);
            return "";
        }
    }
}
