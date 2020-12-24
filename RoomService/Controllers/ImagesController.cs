using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoomService.FileService;
using RoomService.ImagesService;

namespace RoomService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IImagesService _imagesService;

        public ImagesController(IFileService fileService, IImagesService imagesService)
        {
            _fileService = fileService;
            _imagesService = imagesService;
        }

        [HttpPost("addimage")]
        public async Task<string> AddImageAsync([FromForm] AddImageRequestModel imageRequest)
        {
           
            var res = await _imagesService.AddImageAsync(imageRequest);
            return res;
        }

        [HttpPost("addimage")]
        public async Task<string> DeleteImageAsync([FromForm] DeleteImageRequestModel imageRequest)
        {

            var res = await _imagesService.DeleteImageAsync(imageRequest);
            return res;
        }



    }
}
