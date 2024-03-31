using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Features.FacultyApp.Queries.GetAllFacutiesPaging;
using Server.Contracts.Faculties;
using Server.Domain.Common.Constants;

namespace Server.Api.Controllers.ClientApi
{
    public class FacultiesController : ClientApiController
    {
        private readonly IMapper _mapper;
        public FacultiesController(IMediator mediator,IMapper mapper) : base(mediator)
        {
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllFacultiesByPaging([FromQuery] GetAllFacultiesPagingRequest getAllFacultiesPagingRequest)
        {
            var query = _mapper.Map<GetAllFacultiesPagingQuery>(getAllFacultiesPagingRequest);

            var result = await _mediatorSender.Send(query);

            return result.Match(
                successResult => Ok(successResult),
                errors => Problem(errors)
            );
        }
    }
}
