using System.Runtime.CompilerServices;
using ErrorOr;
using MediatR;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;

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
            foreach(var id in request.Ids)
            {
                var contribution = await _unitOfWork.PublicContributionRepository.GetByIdAsync(id);
                var alreadyAllowedContribution = _unitOfWork.PublicContributionRepository.Find(x => x.AllowedGuest == true && x.FacultyId == contribution.FacultyId);
                foreach (var item in alreadyAllowedContribution)
                {
                    item.AllowedGuest = false;
                }

                contribution.AllowedGuest = true;
            }
            await _unitOfWork.CompleteAsync();
            return new ResponseWrapper
            {
                IsSuccessfull = true,
                Messages = new List<string> { $"Perform successfully" }
            };
        }
    }
}
