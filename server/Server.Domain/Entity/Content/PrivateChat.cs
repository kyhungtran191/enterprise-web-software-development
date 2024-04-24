using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Domain.Entity.Content;

[Table("PrivateChats")]
public class PrivateChat
{
    [Key]
    public Guid Id { get; set; }

    public Guid User1Id { get; set; }
    public Guid User2Id { get; set; }
    public DateTime LastActivity { get; set; }
    public DateTime DateCreated { get; set; }
}