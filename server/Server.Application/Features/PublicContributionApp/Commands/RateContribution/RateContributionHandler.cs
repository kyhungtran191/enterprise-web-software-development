using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Content;
using Server.Domain.Entity.Identity;

namespace Server.Application.Features.PublicContributionApp.Commands.RateContribution
{
    public class RateContributionHandler : IRequestHandler<RateContributionCommand, ErrorOr<IResponseWrapper>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        public RateContributionHandler(IUnitOfWork unitOfWork,UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<ErrorOr<IResponseWrapper>> Handle(RateContributionCommand request, CancellationToken cancellationToken)
        {
           var contribution = await _unitOfWork.PublicContributionRepository.GetByIdAsync(request.ContributionId);
            if(contribution is null)
            {
                return Errors.Contribution.NotFoundPublic;
            }
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if(user is null)
            {
                return Errors.User.CannotFound;
            }
            var ratingFromDb =  _unitOfWork.RatingRepository.Find(x=> x.UserId == request.UserId & x.ContributionPublicId == request.ContributionId).FirstOrDefault();
            var averageRating = 0.0;
            if(ratingFromDb != null)
            {
                ratingFromDb.Rating = request.Rating;
                averageRating = await _unitOfWork.RatingRepository.GetAverageRatingAsync(request.ContributionId);
                contribution.AverageRating = averageRating;

                await _unitOfWork.CompleteAsync();
                return new ResponseWrapper
                {
                    Messages = new List<string> { $"Rating success" },
                    IsSuccessfull = true
                };
            }
            var rating = new ContributionPublicRating
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                ContributionPublicId = request.ContributionId,
                Rating = request.Rating
            };
             _unitOfWork.RatingRepository.Add(rating);
            await _unitOfWork.CompleteAsync();
            averageRating = await _unitOfWork.RatingRepository.GetAverageRatingAsync(request.ContributionId);
            contribution.AverageRating = averageRating;

            await _unitOfWork.CompleteAsync();
            return new ResponseWrapper
            {
                Messages = new List<string> { $"Rating success" },
                IsSuccessfull = true
            };

        }
    }
}
