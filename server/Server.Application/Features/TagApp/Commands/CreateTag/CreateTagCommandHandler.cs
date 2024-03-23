using ErrorOr;
using MediatR;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Content;

namespace Server.Application.Features.TagApp.Commands.CreateTag
{
    public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, ErrorOr<IResponseWrapper>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateTagCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<IResponseWrapper>> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            if(await _unitOfWork.TagRepository.GetTagByName(request.TagName) is not null)
            {
                return Errors.Tags.AlreadyExist;
            }

            _unitOfWork.TagRepository.Add(new Tag
            {
                Name = request.TagName,
            });
            await _unitOfWork.CompleteAsync();
            return new ResponseWrapper
            {
                IsSuccessfull = true,
                Messages = new List<string>()
                {
                    "Create new tag successfully"
                }
            };
        }
    }
}
