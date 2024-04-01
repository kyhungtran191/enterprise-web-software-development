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
        [FileValidationFilter(5*1024*1024)]
        public async Task<IActionResult> UploadImages(string type,List<IFormFile> files)
        {
            var path = await _mediaService.UploadFiles(files, type);
            return Ok(new {path });

        }
        [HttpPost("/delete-file")]
        public async Task<IActionResult> DeleteFile(List<string>filePaths)
        {
            await _mediaService.RemoveFile(filePaths);
            return Ok();
        }
        [HttpGet]
        [Route("download-files")]
        public async Task<IActionResult> DownloadFilesAction([FromQuery] List<string> filePaths)
        {
            //try
            //{
            //    var (fileStream, contentType, fileName) = await _mediaService.DownloadFiles(filePaths);

            //    if (fileStream is MemoryStream memoryStream)
            //    {
            //        return File(memoryStream.ToArray(), contentType, fileName);
            //    }
            //    return File(fileStream, contentType, fileName);
                
            //}
            //catch (FileNotFoundException ex)
            //{
            //    return NotFound(ex.Message);
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, $"An error occurred: {ex.Message}");
            //}
            return Ok(_mediaService.GenerateDownloadUrl(filePaths));
        }

    }
}


