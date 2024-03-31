using ErrorOr;
using MediatR;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;

namespace Server.Application.Features.PublicContributionApp.Commands.CreateReadLater
{
    public class CreateReadLaterHandler : IRequestHandler<CreateReadLaterCommand, ErrorOr<IResponseWrapper>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateReadLaterHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<IResponseWrapper>> Handle(CreateReadLaterCommand request, CancellationToken cancellationToken)
        {
            var contribution = await _unitOfWork.PublicContributionRepository.GetByIdAsync(request.ContributionId);
            if(contribution is null)
            {
                return Errors.Contribution.NotFoundPublic;
            }
           
            if(await _unitOfWork.PublicContributionRepository.AlreadyReadLater(contribution,request.UserId))
            {
                var readLater = await _unitOfWork.PublicContributionRepository.GetReadLater(contribution, request.UserId);
                _unitOfWork.PublicContributionRepository.RemoveReadLater(readLater);
                await _unitOfWork.CompleteAsync();
                return new ResponseWrapper
                {
                    IsSuccessfull = true,
                    Messages = new List<string>
                    {
                        $"Remove from read later successfully"
                    }
                };
            }
            await _unitOfWork.PublicContributionRepository.AddToReadLater(contribution, request.UserId);
            await _unitOfWork.CompleteAsync();
            return new ResponseWrapper
            {
                IsSuccessfull = true,
                Messages = new List<string>
                {
                    $"Add to read later successfully"
                }
            };
        }
    }
}
