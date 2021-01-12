using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoomService.Publisher;
using RoomService.RoomModel;
using RoomService.RoomService;

namespace RoomService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IPublisher _publisher;
        public RoomsController(IRoomService roomService, IPublisher publisher)
        {
            _roomService = roomService;
            _publisher = publisher;
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
        public async Task<IActionResult> AddRoomAsync([FromForm] RoomRequestModel room)
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

        [HttpPut("editroom")]
        public async Task<IActionResult> EditRoomAsync([FromForm] EditRoomRequestModel model)
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

        [HttpGet("getallrooms")]
        public async Task<IActionResult> GetAllRoomsAsync()
        {
            var getAllRooms = await _roomService.GetAllRoomsAsync();
            return Ok(getAllRooms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoom(Guid id)
        {
            var room = await _roomService.GetRoom(id);
            return Ok(room);
        }
        [HttpPost("test")]
        public async Task Send()
        {
            var room = "new room";
            await _publisher.Publish(room);
        }
    }
}

