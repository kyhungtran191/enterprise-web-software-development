using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Server.Application.Common.Dtos;
using Server.Application.Wrappers;

namespace Server.Application.Features.ContributionApp.Commands.CreateContribution
{
    public class CreateContributionCommand : IRequest<ErrorOr<IResponseWrapper>>
    {
        public required string Title { get; set; }
        public required string Slug { get; set; }
        public List<FileDto> ThumbnailInfo { get; set; }
        public List<FileDto> FileInfo { get; set; }
        public Guid AcademicYearId { get; set; }
        public Guid FacultyId { get; set; }
        public Guid UserId { get; set; }
    }
}
