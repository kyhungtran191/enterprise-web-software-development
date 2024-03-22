using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Common.Interfaces.Services;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Content;
using Server.Domain.Entity.Identity;

namespace Server.Application.Features.FacultyApp.Commands.DeleteFaculty;

public class DeleteFacultyCommandHanlder : IRequestHandler<DeleteFacultyCommand, ErrorOr<IResponseWrapper>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly UserManager<AppUser> _userManager;

    public DeleteFacultyCommandHanlder(IUnitOfWork unitOfWork,
                                       IDateTimeProvider dateTimeProvider,
                                       UserManager<AppUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _userManager = userManager;
    }

    public async Task<ErrorOr<IResponseWrapper>> Handle(DeleteFacultyCommand request,
                                                        CancellationToken cancellationToken)
    {
        if (await _unitOfWork.FacultyRepository.GetByIdAsync(request.FacultyId) is not Faculty facultyFromDb)
        {
            return Errors.Faculty.CannotFound;
        }

        var hasFacultyInUser = _userManager.Users.SingleOrDefault(x => x.FacultyId == request.FacultyId);

        if (hasFacultyInUser is not null)
        {
            return Errors.Faculty.HasInUser;
        }

        facultyFromDb.DateDeleted = _dateTimeProvider.UtcNow;

        await _unitOfWork.CompleteAsync();

        return new ResponseWrapper
        {
            IsSuccessfull = true,
            Messages = new List<string>
            {
                "Delete faculty successfull!",
                "The faculty is available in 30 days."
            },
        };
    }
}