using Server.Contracts.Common.report;
using Server.Contracts.Contributions.report;

namespace Server.Application.Common.Interfaces.Services;

public interface IContributionService
{
    Task<ReportChartResponse<TotalContributionsPerFacultyData>> GetContributionsWithinEachFacultyForEachAcademicYearReport();

    Task<ReportChartResponse<PercentageTotalContributionsPerFacultyPerAcademicYearData>> GetPercentageTotalContributionsPerFacultyPerAcademicYearReport(string academicYearName);
}