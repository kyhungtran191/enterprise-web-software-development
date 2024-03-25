using ErrorOr;
using MediatR;
using Server.Application.Wrappers;

namespace Server.Application.Features.ContributionApp.Commands.CreateContribution
{
    public class CreateContributionCommand : IRequest<ErrorOr<IResponseWrapper>>
    {
        public required string Title { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? FilePath { get; set; }
        public Guid AcademicYearId { get; set; }
        public Guid FacultyId { get; set; }
        public Guid UserId { get; set; }
        public List<Guid>? TagId { get; set; }
    }
}
