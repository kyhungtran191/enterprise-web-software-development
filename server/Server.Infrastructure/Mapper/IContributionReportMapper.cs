using Server.Application.Common.Dtos.Contributions;
using Server.Contracts.Common.report;
using Server.Contracts.Contributions.report;

namespace Server.Infrastructure.Mapper;

public interface IContributionReportMapper
{
    Task<ReportChartResponse<TotalContributionsPerFacultyData>> MapToContributionsWithinEachFacultyForEachAcademicYear(List<ContributionsWithinEachFacultyForEachAcademicYearDto> data);
}

