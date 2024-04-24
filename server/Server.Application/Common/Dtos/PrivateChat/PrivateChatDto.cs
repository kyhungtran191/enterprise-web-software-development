namespace Server.Application.Common.Dtos.PrivateChat;

public class PrivateChatDto
{
    public Guid Id { get; set; }

    public Guid User1Id { get; set; }
    public Guid User2Id { get; set; }
    public DateTime LastActivity { get; set; }
    public DateTime DateCreated { get; set; }
}