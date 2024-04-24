namespace Server.Application.Common.Dtos.PrivateChat;

public class PrivateMessageDto
{
    public Guid SenderId { get; set; }
    public string? AvatarSender { get; set; }
    public Guid ReceiverId { get; set; }
    public string? AvatarReceiver { get; set; }
    public DateTime DateCreated { get; set; }
    public string? Content { get; set; }    
    public string? ChatId { get; set; }
}