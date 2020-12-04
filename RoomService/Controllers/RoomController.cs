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

        [HttpDelete("deleteroom")]
        public async Task<IActionResult> DeleteRoomAsync(Guid id)
        {
            var resp = await _roomService.DeleteRoomAsync(id);
            return Ok(resp);
        }

        [HttpPost("addroom")]
        public async Task<IActionResult> AddRoomAsync(RoomRequestModel room)
        {
            var resp = await _roomService.AddARoomAsync(room);
            return Ok(resp);
        }

        [HttpPost("updateroom")]
        public async Task<IActionResult> UpdateRoomAsync(UpdateRoomModelRequest model)
        {
            var resp = await _roomService.EditRoomAsync(model);
            return Ok(resp);
        }
    }
}
