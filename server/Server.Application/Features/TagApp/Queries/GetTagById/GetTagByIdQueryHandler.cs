using AutoMapper;
using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.Tags;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Content;

namespace Server.Application.Features.TagApp.Queries.GetTagById
{
    public class GetTagByIdQueryHandler : IRequestHandler<GetTagByIdQuery, ErrorOr<IResponseWrapper<TagDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTagByIdQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public async Task<ErrorOr<IResponseWrapper<TagDto>>> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.TagRepository.GetByIdAsync(request.TagId) is not Tag tagFromDb)
            {
                return Errors.Tags.CannotFound;

            }

            return new ResponseWrapper<TagDto>
            {
                IsSuccessfull = true,
                Messages = new List<string> { $"Get Tag '{tagFromDb.Name}' success" },
                ResponseData = _mapper.Map<TagDto>(tagFromDb)
            };
        }
    }
}
