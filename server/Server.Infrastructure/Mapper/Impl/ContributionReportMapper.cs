using Server.Application.Common.Dtos.Contributions.report;
using Server.Application.Common.Interfaces.Persistence;
using Server.Contracts.Common.report;
using Server.Contracts.Contributions.report;
using Server.Domain.Entity.Content;

namespace Server.Infrastructure.Mapper.Impl;

public class ContributionReportMapper : IContributionReportMapper
{
    private readonly IFacultyRepository _facultyRepository;

    public ContributionReportMapper(IFacultyRepository facultyRepository)
    {
        _facultyRepository = facultyRepository;
    }

    public async Task<ReportChartResponse<TotalContributionsPerFacultyData>>
    MapToContributionsWithinEachFacultyForEachAcademicYear(List<ContributionsWithinEachFacultyForEachAcademicYearDto> data)
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

    public ReportChartResponse<PercentageTotalContributionsPerFacultyPerAcademicYearData> MapToPercentageTotalContributionsPerFacultyPerAcademicYearReportChartResponse(List<PercentagesContributionsWithinEachFacultyForEachAcademicYearDto> data)
    {
        var totalData = data.Count();

        var result = new ReportChartResponse<PercentageTotalContributionsPerFacultyPerAcademicYearData>();

        var reportChartResponseWrapper = new ReportChartResponseWrapper<PercentageTotalContributionsPerFacultyPerAcademicYearData>();

        reportChartResponseWrapper.AcademicYear = data[0].AcademicYear;

        for (var i = 0; i < totalData; ++i)
        {
            var contributionsAndFaculty = new PercentageTotalContributionsPerFacultyPerAcademicYearData();
            contributionsAndFaculty.Faculty = data[i].Faculty;
            contributionsAndFaculty.Data = data[i].Percentages;

            reportChartResponseWrapper.DataSets.Add(contributionsAndFaculty);
        }

        result.Response.Add(reportChartResponseWrapper);

        return result;
    }

    public async Task<ReportChartResponse<TotalContributionFollowingStatusDataSet>> MapToTotalContributionFollowingStatusDataResponse(List<TotalContributionFollowingStatusData> data)
    {

        var result = new ReportChartResponse<TotalContributionFollowingStatusDataSet>();
        var allStatuses = ContributionStatusHelper.GetAllStatuses();

        var groupedByYear = data.GroupBy(x => x.AcademicYear)
            .OrderBy(g => g.Key);
       

        foreach (var yearGroup in groupedByYear)
        {
            var reportWrapper = new ReportChartResponseWrapper<TotalContributionFollowingStatusDataSet>
            {
                AcademicYear = yearGroup.Key
            };
            // default all datasets --> data : 0
            var dataSets = allStatuses.Select(s => new TotalContributionFollowingStatusDataSet
            {
                Status = s,
                Data = 0
            }).ToList();

            foreach (var item in yearGroup)
            {
                var statusData = dataSets.Where(s => s.Status == item.Status).ToList();
                foreach (var status in statusData)
                {
                    dataSets.Remove(status);
                    status.Data = item.Data;
                    dataSets.Add(status);

                }
            }
            reportWrapper.DataSets.AddRange(dataSets);
            result.Response.Add(reportWrapper);
        }

        return await Task.FromResult(result);

    }

    public async Task<ReportChartResponse<TotalContributorsPerFacultyData>> MapToTotalContributorsPerEachFacultiesPerEachAcademicYearsResponse(List<TotalContributorsPerEachFacultiesPerEachAcademicYearsDto> data)
    {
        var sumFaculties = await _facultyRepository.Count();
        var totalData = data.Count();

        var result = new ReportChartResponse<TotalContributorsPerFacultyData>();

        for (var i = 0; i < totalData; i += sumFaculties)
        {
            var reportChartResponseWrapper = new ReportChartResponseWrapper<TotalContributorsPerFacultyData>();

            reportChartResponseWrapper.AcademicYear = data[i].AcademicYear;

            for (var j = i; j < i + sumFaculties; ++j)
            {
                var contributorsWithFaculty = new TotalContributorsPerFacultyData();
                contributorsWithFaculty.Faculty = data[j].Faculty;
                contributorsWithFaculty.Data = data[j].Contributors;

                reportChartResponseWrapper.DataSets.Add(contributorsWithFaculty);
            }

            result.Response.Add(reportChartResponseWrapper);
        }

        return result;
    }
}