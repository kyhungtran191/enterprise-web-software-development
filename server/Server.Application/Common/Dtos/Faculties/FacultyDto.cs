namespace Server.Application.Common.Dtos.Faculties;

public class FacultyDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Icon { get; set; } = default!;
    public DateTime? DateEditted { get; set; }
    public DateTime DateCreated { get; set; } = default!;
    public DateTime? DateDeleted { get; set; }
}