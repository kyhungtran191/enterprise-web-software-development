using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;
using MediatR;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Common.Interfaces.Services;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Content;

namespace Server.Application.Features.TagApp.Commands.UpdateTag
{
    internal class UpdateTagCommandHandler : IRequestHandler<UpdateTagCommand, ErrorOr<IResponseWrapper>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;
          public UpdateTagCommandHandler(IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
        {
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
           
        }
        public async Task<ErrorOr<IResponseWrapper>> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.TagRepository.GetByIdAsync(request.TagId) is not Tag tagFromDb)
            {
                return Errors.Tags.CannotFound;
            }

            if (tagFromDb.DateDeleted.HasValue)
            {
                return Errors.Tags.Deleted;
            }

            if (await _unitOfWork.TagRepository.GetTagByName(request.TagName) is not null)
            {
                return Errors.Tags.AlreadyExist;
            }
            tagFromDb.Name = request.TagName;
            tagFromDb.DateEdited = _dateTimeProvider.UtcNow;
            await _unitOfWork.CompleteAsync();
            return new ResponseWrapper
            {
                IsSuccessfull = true,
                Messages = new List<string>
                {
                    "Update tag successfull"
                }
            };
        }
    }
}
