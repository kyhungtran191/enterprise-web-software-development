namespace Server.Application.Common.Dtos.Contributions;

public class ContributionsWithinEachFacultyForEachAcademicYearDto
{
    public String? AcademicYear { get; set; }
    public String? Faculty { get; set; }
    public int TotalContributions { get; set; }
}

