using System.ComponentModel.DataAnnotations;

namespace Server.Domain.Entity;

public class BaseEntity
{
    [Key]
    public Guid Id { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateEdited { get; set; }
    public DateTime? DateDeleted { get; set; }
}
