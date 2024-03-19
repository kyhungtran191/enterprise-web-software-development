using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Server.Application.Common.Extensions;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Identity;

namespace Server.Application.Features.Identity.Users.Commands.CreateUser;

public class CreateUserCommandHandler
    : IRequestHandler<CreateUserCommand, ErrorOr<IResponseWrapper>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IFacultyRepository _facultyRepository;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(UserManager<AppUser> userManager,
                                    IMapper mapper,
                                    IFacultyRepository facultyRepository)
    {
        _userManager = userManager;
        _mapper = mapper;
        _facultyRepository = facultyRepository;
    }

    public async Task<ErrorOr<IResponseWrapper>> Handle(CreateUserCommand request,
                                                    CancellationToken cancellationToken)
    {
        if (await _userManager.FindByEmailAsync(request.Email) is not null)
        {
            return Errors.User.DuplicateEmail;
        }

        var facultyFromDb = _facultyRepository.Find(x => x.Name == request.Faculty);
        var hasFacultyFromDb = facultyFromDb.Any();

        if (!hasFacultyFromDb)
        {
            return Errors.Faculty.CannotFound;
        }

        var newUser = new AppUser();

        _mapper.Map(request, newUser);

        newUser.Faculty = facultyFromDb.SingleOrDefault()?.Name;
        newUser.PasswordHash = new PasswordHasher<AppUser>().HashPassword(newUser, request.Password);

        var result = await _userManager.CreateAsync(newUser);

        if (!result.Succeeded)
        {
            return result.GetIdentityResultErrorDescriptions();
        }

        return new ResponseWrapper
        {
            IsSuccessfull = true,
            Messages = new List<string>
                {
                    "Create new user successfully!"
                }
        };
    }


}