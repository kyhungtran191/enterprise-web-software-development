using Microsoft.EntityFrameworkCore;
using Server.Application.Common.Dtos.PrivateChat;
using Server.Application.Common.Interfaces.Persistence;
using Server.Domain.Entity.Content;

namespace Server.Infrastructure.Persistence.Repositories;

public class PrivateChatRepository : RepositoryBase<PrivateChat, Guid>, IPrivateChatRepository
{
    public readonly AppDbContext _dbContext;
    public PrivateChatRepository(AppDbContext context) : base(context)
    {
        _dbContext = context;
    }

    public async Task<List<PrivateChatDto>> GetAllUsers(string currentUserId)
    {
        var receiverUsers = await _dbContext.PrivateChats
            .Where(privateChat => privateChat.User1Id.ToString() == currentUserId || privateChat.User2Id.ToString() == currentUserId)
            .OrderByDescending(privateChat => privateChat.LastActivity)
            .ToListAsync();

        return receiverUsers
            .Select(x => new PrivateChatDto
            {
                Id = x.Id,
                User1Id = x.User1Id,
                User2Id = x.User2Id,
                DateCreated = x.DateCreated
            }).ToList();
    }

    public async Task<bool> HasConversation(string currentUserId, string receiverId)
    {
         var receiverUsers = await _dbContext.PrivateChats
            .Where(privateChat => 
                    (privateChat.User1Id.ToString() == currentUserId && privateChat.User2Id.ToString() == receiverId)
                 || (privateChat.User1Id.ToString() == receiverId && privateChat.User2Id.ToString() == currentUserId))
            .ToListAsync();

        return receiverUsers.Any();
    }
}