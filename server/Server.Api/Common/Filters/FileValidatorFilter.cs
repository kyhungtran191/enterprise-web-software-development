using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Server.Api.Common.Helper;
using Server.Infrastructure.Services.Media;

namespace Server.Api.Common.Filters
{
    public class FileValidationFilter : ActionFilterAttribute
    {
        private string[] _allowedExtensions = Array.Empty<string>();
        private readonly long _maxSize;

        public FileValidationFilter( long maxSize)
        {
            _maxSize = maxSize;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var services = context.HttpContext.RequestServices;
            var mediaSettings = services.GetService<IOptions<MediaSettings>>()?.Value;
            if (mediaSettings != null && !string.IsNullOrEmpty(mediaSettings.AllowFileTypes))
            {
                _allowedExtensions = mediaSettings.AllowFileTypes.Split(",");
            }

            var filesParam = context.ActionArguments.SingleOrDefault(p => p.Value is List<IFormFile>);
            var files = filesParam.Value as List<IFormFile>;

            if (files == null || !files.Any())
            {
                context.Result = new BadRequestObjectResult("File list is null or empty.");
                return;
            }

            foreach (var file in files)
            {
                if (file.Length == 0)
                {
                    context.Result = new BadRequestObjectResult("One or more files are empty.");
                    return;
                }

                if (!FileValidator.IsFileExtensionAllowed(file, _allowedExtensions))
                {
                    var allowedExtensionsMessage = String.Join(", ", _allowedExtensions).Replace(".", "").ToUpper();
                    context.Result = new BadRequestObjectResult($"Invalid file type. Please upload {allowedExtensionsMessage} files.");
                    return;
                }

                if (!FileValidator.IsFileSizeWithinLimit(file, _maxSize))
                {
                    var mbSize = (double)_maxSize / 1024 / 1024;
                    context.Result = new BadRequestObjectResult($"File size exceeds the maximum allowed size ({mbSize} MB).");
                    return;
                }
            }
        }

    }
}
