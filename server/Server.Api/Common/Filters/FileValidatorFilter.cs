using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Server.Api.Common.Helper;
using Server.Infrastructure.Services.Media;
using System.Diagnostics;
using FluentValidation;

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

            bool anyFileAttempted = false;

            foreach (var arg in context.ActionArguments.Values)
            {
                var fileProperties = arg?.GetType().GetProperties().Where(prop => prop.PropertyType == typeof(IFormFile) || prop.PropertyType == typeof(List<IFormFile>));
                foreach (var prop in fileProperties)
                {
                    if (prop.PropertyType == typeof(IFormFile))
                    {
                        var file = prop.GetValue(arg) as IFormFile;
                        if (file != null)
                        {
                            anyFileAttempted = true;
                            if (!ValidateFile(context, file))
                            {
                                return; 
                            }
                        }
                    }
                    else if (prop.PropertyType == typeof(List<IFormFile>))
                    {
                        var files = prop.GetValue(arg) as List<IFormFile>;
                        if (files != null && files.Any())
                        {
                            anyFileAttempted = true;
                            if (files.Count > 5)
                            {
                                throw new ValidationException("Maximum of 5 files can be submitted.");
                            }

                            foreach (var file in files)
                            {
                                if (!ValidateFile(context, file))
                                {
                                    return; 
                                }
                            }
                        }
                    }
                }
            }

            if (!anyFileAttempted)
            {
                return;
                //throw new Exception("No files submitted");
                //context.Result = new BadRequestObjectResult("No files submitted.");
            }
        }

        private bool ValidateFile(ActionExecutingContext context, IFormFile file)
        {
            //if (file.Length == 0)
            //{

            //    context.Result = new BadRequestObjectResult("One or more files are empty.");
            //    return false;
            //}

            if (!_allowedExtensions.Any(ext => file.FileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
            {
                var allowedExtensionsMessage = String.Join(", ", _allowedExtensions).Replace(".", "").ToUpper();
                throw new ValidationException($"Invalid file type. Allowed extensions: {allowedExtensionsMessage}.");
            }

            if (!FileValidator.IsFileSizeWithinLimit(file, _maxSize))
            {
                var mbSize = (double)_maxSize / 1024 / 1024;
                throw new ValidationException($"File size exceeds the maximum allowed size ({mbSize} MB).");
               
            }
            return true;
        }



    }
}
