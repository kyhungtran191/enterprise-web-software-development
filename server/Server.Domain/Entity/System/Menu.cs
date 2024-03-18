using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Domain.Entity.System
{
    [Table("Menus")]
    public class Menu : BaseEntity
    {
        [MaxLength(255)] public string Title { get; set; } = default!;
        [MaxLength(255)]  public string Url { get; set; } = default!;

    }
}
