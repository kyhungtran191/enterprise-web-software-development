using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Features.ContributionApp.Queries.GetActivityLog;
using Server.Application.Features.ContributionApp.Queries.GetNotCommentContribution;
using Server.Contracts.Contributions;
using Server.Domain.Common.Constants;

namespace Server.Api.Controllers.ManagerApi
{

    [Route("api/manager/[controller]")]
    [Authorize]
    public class ManagerApiController : ApiController
    {
       
        public ManagerApiController(ISender mediatorSender) : base(mediatorSender)
        {
            
        }

    }
}
