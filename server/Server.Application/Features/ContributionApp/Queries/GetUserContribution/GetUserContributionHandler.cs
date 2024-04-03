using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Identity;

namespace Server.Application.Features.ContributionApp.Queries.GetUserContribution
{
    public class GetUserContributionHandler :  IRequestHandler<GetUserContributionQuery, ErrorOr<IResponseWrapper<ContributionDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public GetUserContributionHandler(IUnitOfWork unitOfWork, IMapper mapper,UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<ErrorOr<IResponseWrapper<ContributionDto>>> Handle(GetUserContributionQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
          if (user == null)
          {
              return Errors.User.CannotFound;
          }

          var itemFromDb = await _unitOfWork.ContributionRepository.GetContributionBySlug(request.Slug);
          if (itemFromDb is null)
          {
              return Errors.Contribution.NotFound;
          }
          if (itemFromDb.UserName != user.UserName)
          {
              return Errors.User.NotBelongContribution;
          }
          var result = await _unitOfWork.ContributionRepository.GetContributionOfUser(request.Slug, request.UserId);
          return new ResponseWrapper<ContributionDto>
          {
              IsSuccessfull = true,
              Messages = new List<string> { $"Get contribution of user success" },
              ResponseData = result
          };
        }
    }
}
