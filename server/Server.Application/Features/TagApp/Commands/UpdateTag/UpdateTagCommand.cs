using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;
using MediatR;
using Server.Application.Wrappers;

namespace Server.Application.Features.TagApp.Commands.UpdateTag
{
    public class UpdateTagCommand : IRequest<ErrorOr<IResponseWrapper>>
    {
        public Guid TagId { get; set; }
        public string TagName { get; set; } = null!;
    }
}
