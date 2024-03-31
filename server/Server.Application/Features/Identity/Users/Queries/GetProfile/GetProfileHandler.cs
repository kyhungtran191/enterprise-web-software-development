using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Server.Application.Common.Dtos.Users;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Content;
using Server.Domain.Entity.Identity;

namespace Server.Application.Features.Identity.Users.Queries.GetProfile
{
    public class GetProfileHandler : IRequestHandler<GetProfileQuery, ErrorOr<IResponseWrapper<UserProfileDto>>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProfileHandler(UserManager<AppUser> userManager, IUnitOfWork unitOfWork,IMapper mapper)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ErrorOr<IResponseWrapper<UserProfileDto>>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                return Errors.User.CannotFound;
            }
            var faculty = await _unitOfWork.FacultyRepository.GetByIdAsync(user?.FacultyId ?? Guid.Empty);
            var result = _mapper.Map<UserProfileDto>(user);
            result.Faculty = faculty?.Name ?? string.Empty ;
            return new ResponseWrapper<UserProfileDto>
            {
                IsSuccessfull = true,
                Messages = new List<string> { $"Get user profile successfully" },
                ResponseData = result,
            };

        }
    }
}
