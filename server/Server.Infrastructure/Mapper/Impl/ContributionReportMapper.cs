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

    public async Task<CombineChartResponse<TotalContributionFollowingStatusDataSet>> MapToTotalContributionFollowingStatusDataResponse(List<TotalContributionFollowingStatusData> data)
    {

        var result = new CombineChartResponse<TotalContributionFollowingStatusDataSet>();
       

        var groupedByYear = data.GroupBy(x => x.AcademicYear)
            .OrderBy(g => g.Key);
       

        foreach (var yearGroup in groupedByYear)
        {
            var reportWrapper = new CombineChartResponseWrapper<TotalContributionFollowingStatusDataSet>(){
                AcademicYear = yearGroup.Key,
                TotalLike = yearGroup.Sum(item => item.TotalLike ?? 0),
                TotalComment = yearGroup.Sum(item => item.TotalComment ?? 0),
                TotalContributionApproved = yearGroup.Sum(item => item.TotalContributionApproved ?? 0),
                AverageRating = yearGroup.Average(item => item.AverageRating ?? 0)
            };


            var dataSets = new List<TotalContributionFollowingStatusDataSet>();

            
            dataSets.Add(new TotalContributionFollowingStatusDataSet { Status = "Pending", Data = yearGroup.Sum(item => item.Pending ?? 0) });
            dataSets.Add(new TotalContributionFollowingStatusDataSet { Status = "Approve", Data = yearGroup.Sum(item => item.Approve ?? 0) });
            dataSets.Add(new TotalContributionFollowingStatusDataSet { Status = "Reject", Data = yearGroup.Sum(item => item.Reject ?? 0) });

           
            reportWrapper.DataSets.AddRange(dataSets);

    
            result.Response.Add(reportWrapper); ;
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