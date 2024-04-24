using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.Users;
using Server.Application.Wrappers.PagedResult;
using Server.Application.Wrappers;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Server.Application.Common.Interfaces.Persistence;
using Server.Domain.Common.Constants;
using Server.Domain.Entity.Identity;
using Server.Application.Common.Interfaces.Services;

namespace Server.Application.Features.Identity.Users.Queries.GetAllGuestsPaging
{
    public class GetAllGuestsPagingHandler : IRequestHandler<GetAllGuestsPagingQuery, ErrorOr<IResponseWrapper<PagedResult<UserDto>>>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IFacultyRepository _facultyRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public GetAllGuestsPagingHandler(UserManager<AppUser> userManager, IMapper mapper, IFacultyRepository facultyRepository, IUserService userService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _facultyRepository = facultyRepository;
            _userService = userService;
        }
        public async Task<ErrorOr<IResponseWrapper<PagedResult<UserDto>>>> Handle(GetAllGuestsPagingQuery request, CancellationToken cancellationToken)
        {
            var query = await _userManager.GetUsersInRoleAsync(Roles.Guest);
            query = query.Where(x => x.FacultyId == _userService.FacultyId).ToList();

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                query = query.Where(
                        user => user.UserName!.Contains(request.Keyword)
                                || user.Email!.Contains(request.Keyword)
                                || user.FirstName.Contains(request.Keyword)
                                || user.LastName.Contains(request.Keyword)
                                || user.PhoneNumber!.Contains(request.Keyword)).ToList();
                
            }

            var count = query.Count;

            var skipPage = (request.PageIndex - 1) * request.PageSize;

            query =
                query
                    .Skip(skipPage)
                    .Take(request.PageSize)
                    .OrderByDescending(x => x.DateCreated).ToList();

            var result = query.Select(x => new UserDto
            {
                Avatar = x.Avatar,
                DateCreated = x.DateCreated!,
                Dob = x.Dob,
                Email = x.Email,
                FirstName = x.FirstName,
                Faculty =   _facultyRepository.GetByIdAsync(x.FacultyId.Value).GetAwaiter().GetResult().Name,
                Id = x.Id,
                IsActive = x.IsActive,
                LastName = x.LastName,
                UserName = x.UserName,
                Roles = _userManager.GetRolesAsync(x!).GetAwaiter().GetResult(),
            }).ToList();
            return new ResponseWrapper<PagedResult<UserDto>>
            {
                IsSuccessfull = true,
                ResponseData = new PagedResult<UserDto>
                {
                    CurrentPage = request.PageIndex,
                    PageSize = request.PageSize,
                    Results = result,
                    RowCount = count,
                }
            };
        }
    }
}
