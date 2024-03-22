namespace Server.Contracts.Faculties;

public class UpdateFacultyRequest
{
    public Guid FacultyId { get; set; }
    public string NewFacultyName { get; set; } = default!;
    public string Icon { get; set; } = default!;
}

