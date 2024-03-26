using Microsoft.AspNetCore.Http;

namespace Server.Application.Common.Interfaces.Services
{
    public interface IMediaService
    {
        Task<List<string>> UploadFiles(List<IFormFile> files,string type);
        //Task<(byte[], string, string)> DownloadFile(string FileName);
    }
}
