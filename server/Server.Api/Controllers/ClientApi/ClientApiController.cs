using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.Contracts.Identity.Users;

namespace Server.Api.Controllers.ClientApi
{
    [Route("api/client/[controller]")]
    
    public class ClientApiController : ApiController
    {
        public ClientApiController(ISender mediatorSender) : base(mediatorSender)
        {
        }

       
    }
}
