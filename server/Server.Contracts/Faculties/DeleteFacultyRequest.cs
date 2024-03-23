using Microsoft.AspNetCore.Mvc;

namespace Server.Contracts.Faculties;

public class DeleteFacultyRequest
{
    [FromRoute]
    public Guid FacultyId { get; set; }
}