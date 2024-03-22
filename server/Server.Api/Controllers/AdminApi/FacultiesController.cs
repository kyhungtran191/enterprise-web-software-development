using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Features.FacultyApp.Commands.CreateFaculty;
using Server.Application.Features.FacultyApp.Commands.DeleteFaculty;
using Server.Application.Features.FacultyApp.Commands.UpdateFaculty;
using Server.Application.Features.FacultyApp.Queries.GetAllFacutiesPaging;
using Server.Application.Features.FacultyApp.Queries.GetFacultyByName;
using Server.Contracts.Faculties;

namespace Server.Api.Controllers.AdminApi;

public class FacultiesController : AdminApiController
{
    private readonly IMapper _mapper;
    public FacultiesController(ISender mediatorSender, IMapper mapper) : base(mediatorSender)
    {
        _mapper = mapper;
    }

    [HttpGet]
    [Route("{FacultyId}")]
    public async Task<IActionResult> GetFacultyById([FromRoute] GetFacultyByIdRequest getFacultyByNameRequest)
    {
        var query = _mapper.Map<GetFacultyByIdQuery>(getFacultyByNameRequest);

        var result = await _mediatorSender.Send(query);

        return result.Match(
            successResult => Ok(successResult),
            errors => Problem(errors)
        );
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

    [HttpPost]
    public async Task<IActionResult> CreateNewFaculty(CreateFacultyRequest createFacultyRequest)
    {
        var command = _mapper.Map<CreateFacultyCommand>(createFacultyRequest);

        var result = await _mediatorSender.Send(command);

        return result.Match(
            successResult => Ok(successResult),
            errors => Problem(errors)
        );
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateFacultyByName(UpdateFacultyRequest updateFacultyRequest)
    {
        var command = _mapper.Map<UpdateFacultyCommand>(updateFacultyRequest);

        var result = await _mediatorSender.Send(command);

        return result.Match(
            successResult => Ok(successResult),
            errors => Problem(errors)
        );
    }

    [HttpDelete]
    [Route("{facultyId}")]
    public async Task<IActionResult> DeleteFacultyById([FromRoute] DeleteFacultyRequest deleteFacultyRequest)
    {
        var command = _mapper.Map<DeleteFacultyCommand>(deleteFacultyRequest);

        var result = await _mediatorSender.Send(command);

        return result.Match(
            successResult => Ok(successResult),
            errors => Problem(errors)
        );
    }

}