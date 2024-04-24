using Server.Application.Common.Dtos.PrivateChat;
using Server.Domain.Entity.Content;

namespace Server.Application.Common.Interfaces.Persistence;

public interface IPrivateChatRepository : IRepository<PrivateChat, Guid>
{
    Task<List<PrivateChatDto>> GetAllUsers(string currentUserId);

    Task<bool> HasConversation(string currentUserId, string receiverId);
}