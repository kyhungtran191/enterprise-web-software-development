using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Domain.Entity.Content;

[Table("PrivateMessages")]
public class PrivateMessage
{
    [Key]
    public Guid Id { get; set; }
    public Guid ChatId { get; set; }
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public string? Content { get; set; }
    public DateTime DateCreated { get; set; }
    public bool HasRead { get; set; }    
}

