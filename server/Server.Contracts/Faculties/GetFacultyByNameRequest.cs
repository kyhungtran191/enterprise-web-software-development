using Microsoft.AspNetCore.Mvc;

namespace Server.Contracts.Faculties;

public class GetFacultyByIdRequest
{
    [FromRoute]    
    public Guid FacultyId { get; set; }
}

