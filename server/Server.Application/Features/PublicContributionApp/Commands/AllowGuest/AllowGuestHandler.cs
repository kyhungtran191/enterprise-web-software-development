using ErrorOr;
using MediatR;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Content;


namespace Server.Application.Features.PublicContributionApp.Commands.AllowGuest
{
    public class AllowGuestHandler : IRequestHandler<AllowGuestCommand, ErrorOr<IResponseWrapper>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public AllowGuestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<IResponseWrapper>> Handle(AllowGuestCommand request, CancellationToken cancellationToken)
        {
            if (request.Ids.Count > 0)
            {
                foreach (var id in request.Ids)
                {
                    var publicContribution = await _unitOfWork.PublicContributionRepository.GetByIdAsync(id);
                    var contribution = await _unitOfWork.ContributionRepository.GetByIdAsync(id);
                    if (contribution is null)
                    {
                        return Errors.Contribution.NotFound;
                    }

                    if (contribution.FacultyId != request.FacultyId)
                    {
                        return Errors.Contribution.NotBelongFaculty;
                    }

                    if (!contribution.PublicDate.HasValue)
                    {
                        return Errors.Contribution.NotFoundPublic;
                    }

                    if (contribution.AllowedGuest)
                    {
                        contribution.AllowedGuest = false;
                    }
                    else
                    {
                        contribution.AllowedGuest = true;
                    }

                    if (publicContribution.AllowedGuest)
                    {
                        publicContribution.AllowedGuest = false;
                    }
                    else
                    {
                        publicContribution.AllowedGuest = true;
                    }
                }
                await _unitOfWork.CompleteAsync();
                return new ResponseWrapper
                {
                    IsSuccessfull = true,
                    Messages = new List<string> { $"Perform successfully" }
                };
            }
            var alreadyAllowedPublicContribution = _unitOfWork.PublicContributionRepository.Find(x => x.AllowedGuest == true && x.FacultyId == request.FacultyId);
            var alreadyAllowedContribution = _unitOfWork.ContributionRepository.Find(x =>
                x.AllowedGuest == true && x.FacultyId == request.FacultyId);
            foreach (var item in alreadyAllowedPublicContribution)
            {
                item.AllowedGuest = false;
            }

            foreach (var item in alreadyAllowedContribution)
            {
                item.AllowedGuest = false;
            }

            //if (request.Ids.Count > 0)
            //{
            //    List<ContributionPublic> publicContributionsToUpdate = new List<ContributionPublic>();
            //    List<Contribution> contributionsToUpdate = new List<Contribution>();
            //    foreach (var id in request.Ids)
            //    {
            //        var publicContribution = await _unitOfWork.PublicContributionRepository.GetByIdAsync(id);
            //        var contribution = await _unitOfWork.ContributionRepository.GetByIdAsync(id);
            //        if (contribution is null)
            //        {
            //            return Errors.Contribution.NotFound;
            //        }

            //        if (contribution.FacultyId != request.FacultyId)
            //        {
            //            return Errors.Contribution.NotBelongFaculty;
            //        }

            //        if (!contribution.PublicDate.HasValue)
            //        {
            //            return Errors.Contribution.NotFoundPublic;
            //        }
            //        if (contribution.PublicDate.HasValue)
            //        {
            //            contributionsToUpdate.Add(contribution);
            //        }
            //        if (publicContribution is not null)
            //        {
            //            publicContributionsToUpdate.Add(publicContribution);
            //        }

            //    }



            //    foreach (var contribution in contributionsToUpdate)
            //    {
            //        contribution.AllowedGuest = true;
            //    }

            //    foreach (var publicContribution in publicContributionsToUpdate)
            //    {
            //        publicContribution.AllowedGuest = true;
            //    }
            //}
           
            await _unitOfWork.CompleteAsync();
            return new ResponseWrapper
            {
                IsSuccessfull = true,
                Messages = new List<string> { $"Perform successfully" }
            };
        }
    }
}
