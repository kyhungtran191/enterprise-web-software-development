using AutoMapper;
using ErrorOr;
using MediatR;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Common.Interfaces.Services;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;

namespace Server.Application.Features.ContributionApp.Commands.UpdateContribution
{
    public class UpdateContributionCommandHandler : IRequestHandler<UpdateContributionCommand,ErrorOr<IResponseWrapper>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;
        public UpdateContributionCommandHandler(IUnitOfWork unitOfWork,IMapper mapper, IDateTimeProvider dateTimeProvider)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;

        }

        public async Task<ErrorOr<IResponseWrapper>> Handle(UpdateContributionCommand request, CancellationToken cancellationToken)
        {
            var itemFromDb  =  await _unitOfWork.ContributionRepository.GetByIdAsync(request.ContributionId);
            if(itemFromDb is null)
            {
                return Errors.Contribution.NotFound;
                
            }
            if (itemFromDb.DateDeleted.HasValue)
            {
                return Errors.Contribution.Deleted;
            }
            _mapper.Map(request,itemFromDb);
            itemFromDb.DateEdited = _dateTimeProvider.UtcNow;
            await _unitOfWork.CompleteAsync();
            return new ResponseWrapper
            {
                IsSuccessfull = true,
                Messages = new List<string>
                {
                    "Update contribution successfully!"
                }
            };
            
        }
    }
}
