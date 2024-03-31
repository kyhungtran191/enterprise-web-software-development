using ErrorOr;
using MediatR;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Server.Application.Features.PublicContributionApp.Commands.CreateFavorite
{
    public class CreateFavoriteHandler : IRequestHandler<CreateFavoriteCommand, ErrorOr<IResponseWrapper>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateFavoriteHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<IResponseWrapper>> Handle(CreateFavoriteCommand request, CancellationToken cancellationToken)
        {
            var contribution = await _unitOfWork.PublicContributionRepository.GetByIdAsync(request.ContributionId);
            if (contribution is null)
            {
                return Errors.Contribution.NotFoundPublic;
            }

            if (await _unitOfWork.PublicContributionRepository.AlreadyFavorite(contribution, request.UserId))
            {
                var favorite = await _unitOfWork.PublicContributionRepository.GetFavorite(contribution, request.UserId);
                _unitOfWork.PublicContributionRepository.RemoveFavorite(favorite);
                await _unitOfWork.CompleteAsync();
                return new ResponseWrapper
                {
                    IsSuccessfull = true,
                    Messages = new List<string>
                    {
                        $"Remove from favorite list successfully"
                    }
                };
            }
            await _unitOfWork.PublicContributionRepository.AddToFavorite(contribution, request.UserId);
            await _unitOfWork.CompleteAsync();
            return new ResponseWrapper
            {
                IsSuccessfull = true,
                Messages = new List<string>
                {
                    $"Add to favorite list successfully"
                }
            };
        }
    }
}
