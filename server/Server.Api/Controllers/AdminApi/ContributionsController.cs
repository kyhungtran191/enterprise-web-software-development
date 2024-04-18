using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Common.Extensions;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Common.Interfaces.Services;
using Server.Application.Features.ContributionApp.Commands.DeleteContribution;
using Server.Application.Features.ContributionApp.Queries.DownloadFile;
using Server.Application.Features.ContributionApp.Queries.GetAllContributionsPaging;
using Server.Application.Features.ContributionApp.Queries.GetContributionByTitle;
using Server.Contracts.Contributions;
using Server.Domain.Common.Constants;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Identity;

namespace Server.Api.Controllers.AdminApi
{
    public class ContributionsController : AdminApiController
    {
        private readonly IMapper _mapper;
        private readonly IContributionService _contributionService;
        private readonly IAcademicYearRepository _academicYearRepository;
        private readonly UserManager<AppUser> _userManager;

        public ContributionsController(ISender _mediator,
                                    IMapper mapper,
                                    IContributionService contributionService,
                                    IAcademicYearRepository academicYearRepository, UserManager<AppUser> userManager) : base(_mediator)
        {
            _mapper = mapper;
            _contributionService = contributionService;
            _academicYearRepository = academicYearRepository;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("{Slug}")]
        [Authorize(Permissions.Contributions.View)]
        public async Task<IActionResult> GetContributionBySlug([FromRoute] GetContributionBySlugRequest getContributionBySlugRequest)
        {
            var query = _mapper.Map<GetContributionBySlugQuery>(getContributionBySlugRequest);
            var result = await _mediatorSender.Send(query);
            return result.Match(result => Ok(result), errors => Problem(errors));

        }

        [HttpGet]
        [Route("paging")]
        [Authorize(Permissions.Contributions.View)]
        public async Task<IActionResult> GetAllContributionsPaging([FromQuery] GetAllContributionPagingRequest getAllContributionPagingRequest)
        {
            var query = _mapper.Map<GetAllContributionsPagingQuery>(getAllContributionPagingRequest);
            var result = await _mediatorSender.Send(query);
            return result.Match(result => Ok(result), errors => Problem(errors));
        }


        [HttpDelete]
        [Authorize(Permissions.Contributions.Delete)]
        public async Task<IActionResult> DeleteContribution(DeleteContributionRequest deleteContributionRequest)
        {
            var command = _mapper.Map<DeleteContributionCommand>(deleteContributionRequest);
            var result = await _mediatorSender.Send(command);
            return result.Match(result => Ok(result), errors => Problem(errors));

        }

        [HttpGet("download-file/{ContributionId}")]
        [Authorize(Permissions.Contributions.Approve)]
        public async Task<IActionResult> DownloadFile([FromRoute] DownloadFileRequest request)
        {
            var query = _mapper.Map<DownloadFileQuery>(request);
            var result = await _mediatorSender.Send(query);
            return result.Match(result => Ok(result), errors => Problem(errors));


        }

        [HttpGet("report-contributions-within-each-faculty-for-each-academic-year")]
        [Authorize(Permissions.Contributions.View)]
        public async Task<IActionResult> GetContributionsWithinEachFacultyForEachAcademicYearsReport()
        {
            var result = await _contributionService.GetContributionsWithinEachFacultyForEachAcademicYearReport();
            return Ok(result);
        }
        
        [HttpGet("report-total-contributors-per-each-faculties-for-each-academic-years")]
        [Authorize(Permissions.Contributions.View)]
        public async Task<IActionResult> GetTotalContributorsPerEachFactultiesForEachAcademicYearsReport()
        {
            var result = await _contributionService.GetTotalContributorsPerEachFacultiesPerEachAcademicYearsDto();
            return Ok(result);
        }
        
        [HttpGet]
        [Route("report-percentages-contributions-within-each-faculty-by-academic-year/{academicYearName}")]
        [Authorize(Permissions.Contributions.View)]
        public async Task<IActionResult> GetPercentagesContributionsWithinEachFacultyByAcademicYearReport([FromRoute] string academicYearName)
        {
            if (await _academicYearRepository.GetAcademicYearByName(academicYearName) is null)
            {
                return ProblemWithError(Errors.AcademicYear.NotFound);
            }

            var result = await _contributionService.GetPercentageTotalContributionsPerFacultyPerAcademicYearReport(academicYearName);
            return Ok(result);
        }

        [HttpGet]
        [Route("report-total-contributions-following-status-for-each-academic-years")]
        public async Task<IActionResult> GetContributionsFollowingStatusForEachAcademicYearReport()
        {
            var userId = User.GetUserId();
            if (await _userManager.FindByIdAsync(userId.ToString()) is null)
            {
                return ProblemWithError(Errors.User.CannotFound);
            }

            var result =
                await _contributionService
                    .GetContributionsFollowingStatusForEachAcademicYearOfCurrentUserReport(userId);
            return Ok(result);
        }
        private IActionResult ProblemWithError(Error error)
            => Problem(new List<Error> { error });
    }
}
