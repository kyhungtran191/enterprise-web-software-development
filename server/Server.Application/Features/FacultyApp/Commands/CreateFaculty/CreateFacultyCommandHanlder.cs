using ErrorOr;
using MediatR;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Content;

namespace Server.Application.Features.FacultyApp.Commands.CreateFaculty;

public class CreateFacultyCommandHanlder : IRequestHandler<CreateFacultyCommand, ErrorOr<IResponseWrapper>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateFacultyCommandHanlder(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<IResponseWrapper>> Handle(CreateFacultyCommand request,
                                                        CancellationToken cancellationToken)
    {
        if (await _unitOfWork.FacultyRepository.GetFacultyByName(request.FacultyName) is not null)
        {
            return Errors.Faculty.AlreadyExisting;
        }

        _unitOfWork.FacultyRepository.Add(new Faculty
        {
            Name = request.FacultyName,
            Icon = request.Icon,
        });
        
        await _unitOfWork.CompleteAsync();

        return new ResponseWrapper
        {
            IsSuccessfull = true,
            Messages = new List<string> 
            {
                "Create new faculty successfull!"
            },
        };
    }
}