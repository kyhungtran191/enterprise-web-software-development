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
    public class GetCoordinatorContributionHandler : IRequestHandler<GetCoordinatorContributionQuery, ErrorOr<IResponseWrapper<ContributionWithCommentDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetCoordinatorContributionHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            
        
        }
        public async Task<ErrorOr<IResponseWrapper<ContributionWithCommentDto>>> Handle(GetCoordinatorContributionQuery request, CancellationToken cancellationToken)
        {
           
            var item = await _unitOfWork.ContributionRepository.GetContributionOfFaculty(request.Slug,request.FacultyName);
            if(item is null)
            {
                return Errors.Contribution.NotFound;
            }
            var comments = await _unitOfWork.CommentRepository.GetCommentByContribution(item.Id);

            var result = _mapper.Map<ContributionWithCommentDto>(item);
            result.Comments = comments;
            return new ResponseWrapper<ContributionWithCommentDto>
            {
                IsSuccessfull = true,
                Messages = new List<string> { $"Get contribution of user success" },
                ResponseData = result
            };
        }
    }
}
