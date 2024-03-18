using MediatR;

namespace Server.Api.Controllers.ClientApi;

public class ContributionPublicController : ApiController
{
    public ContributionPublicController(ISender mediatorSender) : base(mediatorSender)
    {
    }
}