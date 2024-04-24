using Server.Domain.Entity.Content;

namespace Server.Application.Common.Interfaces.Persistence;

public interface IPrivateMessagesRepository : IRepository<PrivateMessage, Guid>
{
    
}