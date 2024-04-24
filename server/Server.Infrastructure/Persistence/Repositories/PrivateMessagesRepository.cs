using Server.Application.Common.Interfaces.Persistence;
using Server.Domain.Entity.Content;

namespace Server.Infrastructure.Persistence.Repositories;

public class PrivateMessagesRepository : RepositoryBase<PrivateMessage, Guid>, IPrivateMessagesRepository
{
    public PrivateMessagesRepository(AppDbContext context) : base(context)
    {
    }
}