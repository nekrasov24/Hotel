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
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;
        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoomAsync(Guid id)
        {
            try
            {
                var resp = await _roomService.DeleteRoomAsync(id);
                return Ok(resp);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPost("addroom")]
        public async Task<IActionResult> AddRoomAsync(RoomRequestModel room)
        {
            try
            {
                var resp = await _roomService.AddARoomAsync(room);
                return Ok(resp);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("updateroom")]
        public async Task<IActionResult> UpdateRoomAsync(UpdateRoomModelRequest model)
        {
            try
            {
                var resp = await _roomService.EditRoomAsync(model);
                return Ok(resp);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
