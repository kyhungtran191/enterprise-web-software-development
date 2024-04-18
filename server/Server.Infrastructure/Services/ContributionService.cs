using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Common.Interfaces.Services;
using Server.Contracts.Common.report;
using Server.Contracts.Contributions.report;
using Server.Infrastructure.Mapper;

namespace Server.Infrastructure.Services;

public class ContributionService : IContributionService
{
    private readonly IConfiguration _configuration;
    private readonly IContributionReportMapper _contributionReportMapper;

    public ContributionService(IConfiguration configuration, IContributionReportMapper contributionReportMapper)
    {
        _configuration = configuration;
        _contributionReportMapper = contributionReportMapper;
    }

    public async Task<ReportChartResponse<TotalContributionsPerFacultyData>> GetContributionsWithinEachFacultyForEachAcademicYearReport()
    {
        using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        if (conn.State == ConnectionState.Open)
        {
            await conn.OpenAsync();
        }

        var sql = @"
                SELECT ay.Name AS AcademicYear,
                    f.Name AS Faculty,	   	   
                    COALESCE(count(c.Id), 0) AS TotalContributions	   
                FROM AcademicYears ay
                CROSS JOIN Faculties f
                LEFT JOIN Contributions c ON c.AcademicYearId = ay.Id AND c.FacultyId = f.Id
                GROUP BY ay.Name, f.Name
                ORDER BY ay.Name, f.Name;
            ";

        var items = await conn.QueryAsync<ContributionsWithinEachFacultyForEachAcademicYearDto>(sql: sql);

        return await _contributionReportMapper.MapToContributionsWithinEachFacultyForEachAcademicYear(items.AsList());
    }




}