using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using Server.Application.Common.Dtos;
using Server.Application.Common.Interfaces.Services;
using System.IO.Compression;
using System.Net.Http.Headers;

namespace Server.Infrastructure.Services.Media
{
    public class MediaService : IMediaService
    {
        private readonly IWebHostEnvironment _hostingEnv;
        private readonly MediaSettings _settings;
        private readonly IDateTimeProvider _dateTimeProvider;

        public MediaService(IWebHostEnvironment hostingEnv, IOptions<MediaSettings> settings, IDateTimeProvider dateTimeProvider)
        {
            _hostingEnv = hostingEnv;
            _settings = settings.Value;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<List<FileDto>> UploadFiles( List<IFormFile> files,string type)
        {
            
         
            var now = _dateTimeProvider.UtcNow;
            var fileInfos = new List<FileDto>();
            if (files is null || files.Contains(null))
            {
                return new List<FileDto>{};
            }
            foreach (var file in files)
            {
                var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition)?.FileName?.Trim('"');
                var imageFolder = $@"\{_settings.MediaFolder}\{type}\{now:MMyyyy}\{now:D}";
                var folder = _hostingEnv.WebRootPath + imageFolder;
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                var filePath = Path.Combine(folder, filename ?? string.Empty);
                using var fs = File.Create(filePath);
                file.CopyTo(fs);
                fs.Flush();
                var path = Path.Combine(imageFolder, filename ?? string.Empty).Replace("\\", "/");
                var fileInfo = new FileDto
                {
                    Path = path,
                    Type = type,
                    Name = filename
                };
                fileInfos.Add(fileInfo);
            }
            return fileInfos;

           
        }
        public Task RemoveFile(List<string> filePaths)
        {
            foreach (var filePath in filePaths)
            {
                var absolutePath = Path.Combine(_hostingEnv.WebRootPath, filePath.TrimStart('\\', '/'));

                if (File.Exists(absolutePath))
                {
                    try
                    {
                        File.Delete(absolutePath);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException($"Error deleting file at {absolutePath}", ex);
                    }
                }
            }
          

            return Task.CompletedTask;
        }
        public async Task<(Stream fileStream, string contentType, string fileName)> DownloadFiles(List<string> filePaths)
        {
            if (filePaths == null || filePaths.Count == 0)
                throw new FileNotFoundException("No file paths provided.");

            if (filePaths.Count == 1)
            {
                var filePath = _hostingEnv.WebRootPath + filePaths[0];
                if (!File.Exists(filePath))
                    throw new FileNotFoundException("File not found.");

                var contentType = "application/octet-stream";
                new FileExtensionContentTypeProvider().TryGetContentType(filePath, out contentType);
                var fileName = Path.GetFileName(filePath);
                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                return (fileStream, contentType, fileName);
            }
           var zipName = $"files_{DateTime.UtcNow:yyyyMMddHHmmss}.zip";
           var memoryStream = new MemoryStream();

            using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                foreach (var relativePath in filePaths)
                {
                    var physicalPath = _hostingEnv.WebRootPath + relativePath;
                    if (File.Exists(physicalPath))
                    {
                        var fileName = Path.GetFileName(physicalPath);
                        var zipEntry = zipArchive.CreateEntry(fileName, CompressionLevel.Fastest);
                        using (var zipEntryStream = zipEntry.Open())
                        using (var fileStream = new FileStream(physicalPath, FileMode.Open, FileAccess.Read))
                        {
                            await fileStream.CopyToAsync(zipEntryStream);
                        }
                    }
                }
            }

            memoryStream.Seek(0, SeekOrigin.Begin);

            return (memoryStream, "application/zip", zipName);
        
        }
    }
}
