using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Server.Api.Controllers.ClientApi
{
    [Route("api/[controller]")]
    
    public class ClientApiController : ApiController
    {
        public ClientApiController(ISender mediatorSender) : base(mediatorSender)
        {
            
        }
    }
}
