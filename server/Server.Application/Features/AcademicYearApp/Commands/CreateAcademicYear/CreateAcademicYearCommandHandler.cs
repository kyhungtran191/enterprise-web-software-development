using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Content;
using Server.Domain.Entity.Identity;

namespace Server.Application.Features.AcademicYearApp.Commands.CreateAcademicYear
{
    public class CreateAcademicYearCommandHandler : IRequestHandler<CreateAcademicYearCommand, ErrorOr<IResponseWrapper>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public CreateAcademicYearCommandHandler(IUnitOfWork unitOfWork, UserManager<AppUser> userManager,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;

        }

        public async Task<ErrorOr<IResponseWrapper>> Handle(CreateAcademicYearCommand request, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.AcademicYearRepository.GetAcademicYearByName(request.AcademicYearName) is not null)
            {
                return Errors.AcademicYear.AlreadyExist;
            }

            if (await _userManager.FindByNameAsync(request.UserNameCreated) is null)
            {
                return Errors.User.CannotFound;
            }

            var newYear = new AcademicYear
            {
                Id = Guid.NewGuid(),
                Name = request.AcademicYearName,
                StartClosureDate = request.StartClosureDate,
                EndClosureDate = request.EndClosureDate,
                FinalClosureDate = request.FinalClosureDate,
                UserNameCreated = request.UserNameCreated,
            };
            _unitOfWork.AcademicYearRepository.Add(newYear);
            await _unitOfWork.CompleteAsync();
            return new ResponseWrapper
            {
                IsSuccessfull = true,
                Messages = new List<string>()
                {
                    "Create new academic year successfully"
                }
            };
        }
    }
}
