using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoomService.RoomModel;
using RoomService.RoomService;

namespace RoomService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        public async Task<IActionResult> DeliteRoomAsync(string number)
        {
            var resp = await _roomService.DeliteRoomAsync(number);
            return Ok(resp);
        }

        public async Task<IActionResult> AddRoomAsync(RoomRequestModel room)
        {
            var resp = await _roomService.AddARoomAsync(room);
            return Ok(resp);
        }
    }
}
