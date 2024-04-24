namespace Server.Application.Common.Dtos.PrivateChat;

public class PrivateChatUserDto
{    
    public string? ChatId { get; set; }
    public string? CurrentUserId { get; set; }
    public Guid ReceiverId { get; set; }
    public string? Username { get; set; }
    public string? Avatar { get; set; }
    public string? Role { get; set; }
    public bool IsOnline { get; set; }
    public List<PrivateMessageDto>? CurrentMessagesReceiver { get; set; }
}