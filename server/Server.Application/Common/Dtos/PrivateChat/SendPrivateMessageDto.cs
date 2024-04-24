namespace Server.Application.Common.Dtos.PrivateChat;

public class SendPrivateMessageDto
{
    public string? SenderId { get; set; }
    public string? ReceiverId { get; set; }
    public string? Content { get; set; }    
    public string? ChatId { get; set; }
}