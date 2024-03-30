using ErrorOr;
using MediatR;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Content;

namespace Server.Application.Features.PublicContributionApp.Commands.LikeContribution
{
    public class LikeContributionHandler : IRequestHandler<LikeContributionCommand, ErrorOr<IResponseWrapper>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public LikeContributionHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<IResponseWrapper>> Handle(LikeContributionCommand request, CancellationToken cancellationToken)
        {
            var contribution = await _unitOfWork.PublicContributionRepository.GetByIdAsync(request.ContributionId);
            if (contribution is null)
            {
                return Errors.Contribution.NotFound;
            }

            if (await _unitOfWork.LikeRepository.AlreadyLike(contribution, request.UserId))
            {
                var like = await _unitOfWork.LikeRepository.GetSpecificLike(contribution, request.UserId);
                _unitOfWork.LikeRepository.Remove(like);
                contribution.LikeQuantity -= 1;
                await _unitOfWork.CompleteAsync();
                return new ResponseWrapper
                {
                    IsSuccessfull = true,
                    Messages = new List<string>
                    {
                        $"Dislike success"
                    }
                };
            }
            _unitOfWork.LikeRepository.Add(new Like { ContributionPublicId = contribution.Id, UserId = request.UserId });
            contribution.LikeQuantity++;
            await _unitOfWork.CompleteAsync();
            return new ResponseWrapper
            {
                IsSuccessfull = true,
                Messages = new List<string>
                 {
                     $"Like success"
                 }
            };
        }
    }
}
