using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Features.AcademicYearApp.Queries.GetAllAcademicYearPaging;
using Server.Contracts.AcademicYears;
using Server.Domain.Common.Constants;

namespace Server.Api.Controllers.ClientApi
{
    public class AcademicYearController : ClientApiController
    {
       private readonly IMapper _mapper;
        public AcademicYearController(IMediator _mediator, IMapper mapper) : base(_mediator)
        {
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAcademicYearByPaging([FromQuery] GetAllYearsPagingRequest getAllYearsPagingRequest)
        {
            var query = _mapper.Map<GetAllAcademicYearsPagingQuery>(getAllYearsPagingRequest);
            var result = await _mediatorSender.Send(query);
            return result.Match(result => Ok(result), errors => Problem(errors));
        }
    }
}
