using AutoMapper;
using ErrorOr;
using MediatR;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Common.Interfaces.Services;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Content;

namespace Server.Application.Features.ContributionApp.Commands.CreateContribution
{
    public class CreateContributionCommandHandler : IRequestHandler<CreateContributionCommand, ErrorOr<IResponseWrapper>>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IDateTimeProvider _dateTimeProvider;

        public CreateContributionCommandHandler(IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
        {
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
        }
        public async Task<ErrorOr<IResponseWrapper>> Handle(CreateContributionCommand request, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.ContributionRepository.GetContributionByTitle(request.Title) is not null)
            {
                return Errors.Contribution.AlreadyExist;
            }

            if (await _unitOfWork.AcademicYearRepository.GetByIdAsync(request.AcademicYearId) is null)
            {
                return Errors.Contribution.AcademicYearNotFound;
            }
            _unitOfWork.ContributionRepository.Add(new Contribution
            {
                AcademicYearId = request.AcademicYearId,
                Title = request.Title,
                FilePath = request.FilePath,
                FacultyId = request.FacultyId,
                UserId = request.UserId,
                Thumbnail = request.ThumbnailUrl,
                IsConfirmed = true,
                SubmissionDate = _dateTimeProvider.UtcNow


            });
            await _unitOfWork.CompleteAsync();
            return new ResponseWrapper
            {
                IsSuccessfull = true,
                Messages = new List<string> { $"Create contribution successfully!" }
            };

        }
    }
}
