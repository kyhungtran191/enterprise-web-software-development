using Server.Application.Common.Interfaces.Persistence;
using Server.Domain.Entity.Content;

namespace Server.Infrastructure.Persistence.Repositories;

public class AnnouncementUserRepository 
    : RepositoryBase<AnnouncementUser, int>, IAnnouncementUserRepository
{
    public AnnouncementUserRepository(AppDbContext context) : base(context)
    {
    }
}