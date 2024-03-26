using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Common.Filters;
using Server.Application.Common.Interfaces.Services;

namespace Server.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestMediaController : ControllerBase
    {
     
        private readonly IMediaService _mediaService;

        public TestMediaController(IMediaService mediaService)
        {
           _mediaService = mediaService;
        }

        [HttpPost]
        [AllowAnonymous]
        [FileValidationFilter(1024*1024)]
        public async Task<IActionResult> UploadImages(string type,List<IFormFile> files)
        {
            var path = await _mediaService.UploadFiles(files, type);
            return Ok(new {path });

        }
    }
}


