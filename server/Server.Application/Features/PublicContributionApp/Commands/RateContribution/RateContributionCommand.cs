using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;
using MediatR;
using Server.Application.Wrappers;

namespace Server.Application.Features.PublicContributionApp.Commands.RateContribution
{
    public class RateContributionCommand : IRequest<ErrorOr<IResponseWrapper>>
    {
        public Guid ContributionId { get; set; }
        public Guid UserId { get; set; }
        public double Rating { get; set; }
    }
}
