using MediatR;

namespace Server.Api.Controllers.AdminApi;

public class FacultiesController : AdminApiController
{
    public FacultiesController(ISender mediatorSender) : base(mediatorSender)
    {
    }

    
}