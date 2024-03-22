using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Server.Api.Controllers;

// [ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorsController : ControllerBase
{
    // [NonAction]    
    [Route("/error")]
    public ActionResult Error()
    {
        Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        // var (statusCode, message) = exception switch
        // {
        //     DuplicateEmailException => (StatusCodes.Status409Conflict, "User already exists"),
        //     _ => (StatusCodes.Status500InternalServerError, "An unexpceted error occured."),
        // };

        // var (statusCode, message) = exception switch
        // {
        //     IServiceException serviceException => (serviceException.StatusCode, serviceException.ErrorMessage),
        //     _ => (HttpStatusCode.InternalServerError, "An unexpceted error occured."),
        // };

        return Problem(title: exception?.Message);
        // return Problem(detail: exception?.Message);
    }
}