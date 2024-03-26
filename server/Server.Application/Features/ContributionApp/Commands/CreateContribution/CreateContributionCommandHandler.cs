using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Common.Interfaces.Services;
using Server.Application.Wrappers;
using Server.Contracts.Common;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Content;
using Server.Domain.Entity.Identity;
using File = Server.Domain.Entity.Content.File;

namespace Server.Application.Features.ContributionApp.Commands.CreateContribution
{
    public class CreateContributionCommandHandler : IRequestHandler<CreateContributionCommand, ErrorOr<IResponseWrapper>>
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEmailService _emailService;
        private readonly UserManager<AppUser> _userManager;

        public CreateContributionCommandHandler(IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider, UserManager<AppUser> userManager, IEmailService emailService) 
        {
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
            _emailService = emailService;
            _userManager = userManager;
        }

        public async Task<ErrorOr<IResponseWrapper>> Handle(CreateContributionCommand request,
            CancellationToken cancellationToken)
        {
            if (await _unitOfWork.ContributionRepository.GetContributionByTitle(request.Title) is not null)
            {
                return Errors.Contribution.AlreadyExist;
            }

            if (await _unitOfWork.AcademicYearRepository.GetByIdAsync(request.AcademicYearId) is null)
            {
                return Errors.Contribution.AcademicYearNotFound;
            }

            var contributon = new Contribution
            {
                AcademicYearId = request.AcademicYearId,
                Title = request.Title,
                Slug = request.Slug,
                FacultyId = request.FacultyId,
                UserId = request.UserId,
                IsConfirmed = true,
                SubmissionDate = _dateTimeProvider.UtcNow

            };
            _unitOfWork.ContributionRepository.Add(contributon);
            await _unitOfWork.CompleteAsync();
            foreach (var info in request.FileInfo)
            {
                _unitOfWork.FileRepository.Add(new File
                {
                    ContributionId = contributon.Id,
                    Path = info.Path,
                    Type = info.Type
                });
            }


            foreach (var info in request.ThumbnailInfo)
            {
                _unitOfWork.FileRepository.Add(new File
                {
                    ContributionId = contributon.Id,
                    Path = info.Path,
                    Type = info.Type
                });
            }

            await _unitOfWork.CompleteAsync();

            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                return Errors.User.CannotFound;
            }


             _emailService.SendEmail(new MailRequest
            {
                ToEmail = "nguahoang2003@gmail.com",
                Body = $"User with Id {user.Id} submit new contribution",
                Subject = "NEW CONTRIBUTION"
            });

            return new ResponseWrapper
            {
                IsSuccessfull = true,
                Messages = new List<string> { $"Create contribution successfully!" }
            };

        }
    }
}

