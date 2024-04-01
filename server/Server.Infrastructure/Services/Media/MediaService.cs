using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using Server.Application.Common.Dtos;
using Server.Application.Common.Interfaces.Services;
using System.IO.Compression;
using System.Net.Http.Headers;
using Server.Domain.Common.Constants;

namespace Server.Infrastructure.Services.Media
{
    public class MediaService : IMediaService
    {
        private readonly IWebHostEnvironment _hostingEnv;
        private readonly MediaSettings _settings;
        private readonly CloudinarySettings _cloudinarySettings;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly Cloudinary _cloudinary;
        public MediaService(IWebHostEnvironment hostingEnv, IOptions<MediaSettings> settings, IDateTimeProvider dateTimeProvider, IOptions<CloudinarySettings> cloudinarySettings)
        {
            _hostingEnv = hostingEnv;
            _settings = settings.Value;
            _dateTimeProvider = dateTimeProvider;
            _cloudinarySettings = cloudinarySettings.Value;
            Account account = new Account(_cloudinarySettings.CloudName, _cloudinarySettings.ApiKey,
                _cloudinarySettings.ApiSecret);
            _cloudinary = new Cloudinary(account);
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

        public async Task<List<FileDto>> UploadFileCloudinary(List<IFormFile> files, string type, Guid id)
        {
            var now = _dateTimeProvider.UtcNow;
            var fileInfos = new List<FileDto>();
            if (files is null || files.Contains(null))
            {
                return fileInfos;
            }

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using (var stream = file.OpenReadStream())
                    {
                        var extension = Path.GetExtension(file.FileName).ToLower();
                        var folderPath = type == FileType.Avatar ? $"{type}/user-{id}" : $"{type}/contribution-{id}";
                        UploadResult uploadResult;

                        if (extension == ".jpg" || extension == ".png" || extension == ".gif" || extension == ".bmp" || extension == ".jpeg")
                        {
                           
                            var imageUploadParams = new ImageUploadParams()
                            {
                                File = new FileDescription(file.FileName, stream),
                                Folder = folderPath
                            };
                            uploadResult = await _cloudinary.UploadAsync(imageUploadParams);
                        }
                        else
                        {
                           
                            var rawUploadParams = new RawUploadParams()
                            {
                                File = new FileDescription(file.FileName, stream),
                                Folder = folderPath,
                            };
                            uploadResult = await _cloudinary.UploadAsync(rawUploadParams);
                        }
                        if (uploadResult.Error == null) 
                        {
                            fileInfos.Add(new FileDto
                            {
                                Path = uploadResult.Url.ToString(),
                                Name = file.FileName,
                                Type = type,
                                PublicId = uploadResult.PublicId,
                                Extension = extension
                            });
                        }
                    }
                }
            }

            return fileInfos;

        }
        public async Task RemoveFromCloudinary(List<string> publicIds, List<string> types)
        {
            
            for (int i = 0; i < publicIds.Count; i++)
            {
                var publicId = publicIds[i];
                var type = types[i];

              
                ResourceType resourceType = type == "file" ? ResourceType.Raw : ResourceType.Image;
                try
                {
                    var deletionParams = new DeletionParams(publicId)
                    {
                        ResourceType = resourceType
                    };
                    var deletionResult = await _cloudinary.DestroyAsync(deletionParams);

                    if (deletionResult.Result == "not found")
                    {
                        throw new Exception($"Not Found: {publicId} of type {type}");
                    }
                    else if (deletionResult.Result != "ok")
                    {
                       
                        throw new Exception($"Delete Error: {publicId} of type {type}");
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Deletion error for {publicId} of type {type}: {ex.Message}", ex);
                }
            }
        }
        public string GenerateDownloadUrl(List<string> publicIds)
        {

            var downloadParams = new ArchiveParams();
            downloadParams.PublicIds(publicIds);
            downloadParams.ResourceType("raw");
            if (publicIds.Count > 1)
            {
                return _cloudinary.DownloadArchiveUrl(downloadParams);
            }
               
            
            if (publicIds.Count == 1)
            {
                return $"http://res.cloudinary.com/dlqxj0ibb/raw/upload/v1711946007/{publicIds[0]}";
            }

            return null;
        }
    }

}
