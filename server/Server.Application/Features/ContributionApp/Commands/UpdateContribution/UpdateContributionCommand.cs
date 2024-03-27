using ErrorOr;
using MediatR;
using Server.Application.Wrappers;
using System.ComponentModel.DataAnnotations;

namespace Server.Application.Features.ContributionApp.Commands.UpdateContribution
{
    public class UpdateContributionCommand : IRequest<ErrorOr<IResponseWrapper>>
    {
        public Guid ContributionId { get; set; }
        public string? Title { get; set; }
        public Guid? AcademicYearId { get; set; }
        public string? Thumbnail { get; set; }
        public string? FilePath { get; set; }
    }
}
