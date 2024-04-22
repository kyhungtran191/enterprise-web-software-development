using Server.Application.Common.Interfaces.Persistence;
using Server.Domain.Entity.Content;

namespace Server.Infrastructure.Persistence.Repositories;

public class AnnouncementRepository : RepositoryBase<Announcement, string>, IAnnouncementRepository
{
    public AnnouncementRepository(AppDbContext context) : base(context)
    {
    }
}