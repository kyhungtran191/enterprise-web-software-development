using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Server.Application.Wrappers;

namespace Server.Application.Features.ContributionApp.Commands.UpdateContribution
{
    public class UpdateContributionCommand : IRequest<ErrorOr<IResponseWrapper>>
    {
        public Guid ContributionId { get; set; }
        public required string Title { get; set; }
        public required string Slug { get; set; }
        public IFormFile? Thumbnail { get; set; }
        public List<IFormFile>? Files { get; set; }
        public Guid FacultyId { get; set; }
        public Guid UserId { get; set; }
        //public Guid UserId { get; set; }
        public bool IsConfirmed { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
    }
}
