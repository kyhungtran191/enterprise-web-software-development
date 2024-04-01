using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Features.AcademicYearApp.Commands.CreateAcademicYear;
using Server.Application.Features.AcademicYearApp.Commands.DeleteAcademicYear;
using Server.Application.Features.AcademicYearApp.Commands.UpdateAcademicYear;
using Server.Application.Features.AcademicYearApp.Queries.ActiveYear;
using Server.Application.Features.AcademicYearApp.Queries.GetAcademicYearById;
using Server.Application.Features.AcademicYearApp.Queries.GetAllAcademicYearPaging;
using Server.Contracts.AcademicYears;
using Server.Contracts.Tags;
using Server.Domain.Common.Constants;

namespace Server.Api.Controllers.AdminApi
{

    public class AcademicYearController : AdminApiController
    {
        private readonly IMapper _mapper;

        public AcademicYearController(IMapper mapper, ISender sender) : base(sender)
        {
            _mapper = mapper;
        }
        [HttpGet]
        [Route("{YearId}")]
        [Authorize(Permissions.Tags.View)]
        public async Task<IActionResult> GetAcademicYearById([FromRoute] GetYearByIdRequest getYearByIdRequest)
        {
            var query = _mapper.Map<GetAcademicYearByIdQuery>(getYearByIdRequest);
            var result = await _mediatorSender.Send(query);
            return result.Match(result => Ok(result), errors => Problem(errors));
        }

        [HttpGet]
        [Route("paging")]
        [Authorize(Permissions.AcademicYears.View)]
        public async Task<IActionResult> GetAllAcademicYearByPaging([FromQuery] GetAllYearsPagingRequest getAllYearsPagingRequest)
        {
            var query = _mapper.Map<GetAllAcademicYearsPagingQuery>(getAllYearsPagingRequest);
            var result = await _mediatorSender.Send(query);
            return result.Match(result => Ok(result), errors => Problem(errors));
        }

        [HttpPost]
        [Authorize(Permissions.AcademicYears.Create)]
        public async Task<IActionResult> CreateNewAcademicYear(CreateAcademicYearRequest createAcademicYearRequest)
        {
            var command = _mapper.Map<CreateAcademicYearCommand>(createAcademicYearRequest);
            command.UserNameCreated = User?.Identity.Name;
            var result = await _mediatorSender.Send(command);
            return result.Match(result => Ok(result), errors => Problem(errors));
        }

        [HttpPut]
        [Authorize(Permissions.AcademicYears.Edit)]
        public async Task<IActionResult> UpdateAcademicYear(UpdateAcademicYearRequest updateAcademicYearRequest)
        {
            var command = _mapper.Map<UpdateAcademicYearCommand>(updateAcademicYearRequest);
            var result = await _mediatorSender.Send(command);
            return result.Match(result => Ok(result), errors => Problem(errors));
        }

        [HttpDelete]
        [Authorize(Permissions.AcademicYears.Delete)]
        public async Task<IActionResult> DeleteAcademicYearById(DeleteAcademicYearRequest deleteAcademicYearRequest)
        {
            var command = _mapper.Map<DeleteAcademicYearCommand>(deleteAcademicYearRequest);
            var result = await _mediatorSender.Send(command);
            return result.Match(result => Ok(result), errors => Problem(errors));
        }

        [HttpPost]
        [Route("activate/{YearId}")]
        [Authorize(Permissions.AcademicYears.Create)]
        public async Task<IActionResult> ActiveAcademicYear([FromRoute] ActiveAcademicYearRequest request)
        {
            var command = _mapper.Map<ActiveYearCommand>(request);
            var result = await _mediatorSender.Send(command);
            return result.Match(result => Ok(result), errors => Problem(errors));
        }


    }
}
