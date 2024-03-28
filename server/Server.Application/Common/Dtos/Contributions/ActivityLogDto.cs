using Server.Domain.Entity.Content;

namespace Server.Application.Common.Dtos.Contributions
{
    public class ActivityLogDto
    {
        public string FromStatus { get; set; }
        public string ToStatus { get; set; }
        public DateTime DateCreated { get; set; }
        public string? Description { get; set; }
        public string? UserName { get; set; }

    }
}
