using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoomService.FileService;

namespace RoomService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IFileService _fileService;

        public ImagesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("addimage")]
        public async Task<string> AddImageAsync([FromForm] ImageRequest imageRequest)
        {
            //var res = await _fileService.AddImageAsync(imageRequest);
            return "";
        }

        [HttpGet("{id}")]
        public async Task<string> GetImageAsync(Guid id)
        {
            //var res = await _fileService.GetImageAsync(id);
            return "";
        }
    }
}
