using Microsoft.AspNetCore.Http;
using Server.Application.Common.Dtos;

namespace Server.Application.Common.Interfaces.Services
{
    public interface IMediaService
    {
        Task<List<FileDto>> UploadFiles(List<IFormFile> files,string type);
        //Task<(byte[], string, string)> DownloadFile(string FileName);
    }
}
