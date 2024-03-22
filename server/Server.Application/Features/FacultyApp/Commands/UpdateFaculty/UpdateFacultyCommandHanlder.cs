using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Server.Application.Common.Dtos.Faculties;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Common.Interfaces.Services;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Content;
using Server.Domain.Entity.Identity;

namespace Server.Application.Features.FacultyApp.Commands.UpdateFaculty;

public class UpdateFacultyCommandHanlder : IRequestHandler<UpdateFacultyCommand, ErrorOr<IResponseWrapper>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly UserManager<AppUser> _userManager;

    public UpdateFacultyCommandHanlder(IUnitOfWork unitOfWork,
                                    IDateTimeProvider dateTimeProvider,
                                    UserManager<AppUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _userManager = userManager;
    }

    public async Task<ErrorOr<IResponseWrapper>> Handle(UpdateFacultyCommand request,
                                                        CancellationToken cancellationToken)
    {
        if (await _unitOfWork.FacultyRepository.GetByIdAsync(request.FacultyId) is not Faculty facultyFromDb)
        {
            return Errors.Faculty.CannotFound;
        }
        
        

        facultyFromDb.Name = request.NewFacultyName;
        facultyFromDb.Icon = request.Icon;
        facultyFromDb.DateEdited = _dateTimeProvider.UtcNow;

        await _unitOfWork.CompleteAsync();

        return new ResponseWrapper
        {
            IsSuccessfull = true,
            Messages = new List<string>
            {
                "Update faculty successfull!"
            },
        };
    }
}