using Server.Application.Common.Dtos.Announcement;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Common.Interfaces.Services;
using Server.Application.Wrappers.PagedResult;
using Server.Domain.Entity.Content;

namespace Server.Infrastructure.Services;

public class AnnouncementService : IAnnouncementService
{
    private readonly IUnitOfWork _unitOfWork;

    public AnnouncementService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResult<AnnouncementDto>> GetAllUnreadPaging(Guid userId, int pageIndex, int pageSize)
    {


        var query = from x in (await _unitOfWork.AnnouncementRepository
                                  .GetAllAsync())
                                  .OrderByDescending(x => x.DateCreated)
                    join y in await _unitOfWork.AnnouncementUserRepository.GetAllAsync()
                        on x.Id equals y.AnnouncementId
                        into xy
                    from announUser in xy.DefaultIfEmpty()
                    where (announUser.UserId == null || announUser.UserId == userId)
                    select x;

        int totalRow = query.Count();

        List<AnnouncementDto> announcementViewModels = query.OrderByDescending(x => x.DateCreated)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new AnnouncementDto
            {
                Id = x.Id,
                Content = x.Content,
                DateCreated = x.DateCreated,
                Title = x.Title,
                UserId = userId,
                DateModified = x.DateModified
            }).ToList();

        return new PagedResult<AnnouncementDto>
        {
            Results = announcementViewModels,
            CurrentPage = pageIndex,
            RowCount = totalRow,
            PageSize = pageSize
        };
    }

    public async Task<bool> MarkAsRead(Guid userId, string id)
    {

        bool result = false;

        var announce = 
            _unitOfWork
            .AnnouncementUserRepository
            .Find(x => x.UserId == userId || x.AnnouncementId == id).FirstOrDefault();

        if (announce == null)
        {
            _unitOfWork.AnnouncementUserRepository.Add(new AnnouncementUser
            {
                AnnouncementId = id,
                UserId = userId,
                HasRead = true
            });

            result = true;
        }
        else
        {
            if (announce.HasRead == false)
            {
                announce.HasRead = true;
                result = true;
            }
        }

        await _unitOfWork.CompleteAsync();

        return result;
    }
}