using Server.Application.Common.Dtos.Contributions;
using Server.Application.Common.Interfaces.Persistence;
using Server.Contracts.Common.report;
using Server.Contracts.Contributions.report;

namespace Server.Infrastructure.Mapper.Impl;

public class ContributionReportMapper : IContributionReportMapper
{
    private readonly IFacultyRepository _facultyRepository;

    public ContributionReportMapper(IFacultyRepository facultyRepository)
    {
        _facultyRepository = facultyRepository;
    }

    public async Task<ReportChartResponse<TotalContributionsPerFacultyData>> MapToContributionsWithinEachFacultyForEachAcademicYear(List<ContributionsWithinEachFacultyForEachAcademicYearDto> data)
    {
        var sumFaculties = await _facultyRepository.Count();
        var totalData = data.Count();

        var result = new ReportChartResponse<TotalContributionsPerFacultyData>();

        for (var i = 0; i < totalData; i += sumFaculties)
        {
            var reportChartResponseWrapper = new ReportChartResponseWrapper<TotalContributionsPerFacultyData>();

            reportChartResponseWrapper.AcademicYear = data[i].AcademicYear;

            for (var j = i; j < i + sumFaculties; ++j)
            {
                var contributionsAndFaculty = new TotalContributionsPerFacultyData();
                contributionsAndFaculty.Faculty = data[j].Faculty;
                contributionsAndFaculty.Data = data[j].TotalContributions;

                reportChartResponseWrapper.DataSets.Add(contributionsAndFaculty);
            }

            result.Response.Add(reportChartResponseWrapper);
        }

        return result;
    }
}