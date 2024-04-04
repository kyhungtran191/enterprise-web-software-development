using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Server.Application.Features.ContributionApp.Queries.GetCoordinatorContribution
{
    public class GetCoordinatorContributionHandler : IRequestHandler<GetCoordinatorContributionQuery, ErrorOr<IResponseWrapper<ContributionDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetCoordinatorContributionHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        
        }
        public async Task<ErrorOr<IResponseWrapper<ContributionDto>>> Handle(GetCoordinatorContributionQuery request, CancellationToken cancellationToken)
        {
           
            var result = await _unitOfWork.ContributionRepository.GetContributionOfFaculty(request.Slug,request.FacultyName);
            if(result is null)
            {
                return Errors.Contribution.NotFound;
            }
            return new ResponseWrapper<ContributionDto>
            {
                IsSuccessfull = true,
                Messages = new List<string> { $"Get contribution of user success" },
                ResponseData = result
            };
        }
    }
}
