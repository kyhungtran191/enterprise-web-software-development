using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Server.Application.Common.Dtos.Contributions.report;
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

    public async Task<ReportChartResponse<TotalContributionFollowingStatusDataSet>> GetContributionsFollowingStatusForEachAcademicYearOfCurrentUserReport(Guid currentUserId)
    {
        using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        if (conn.State == ConnectionState.Open)
        {
            await conn.OpenAsync();
        }

        var sql = @"SELECT 
                    ay.Name AS AcademicYear,
                    CASE cs.Status
                        WHEN 0 THEN 'Pending'
                        WHEN 1 THEN 'Approve'
                        WHEN 2 THEN 'Reject'
                    END AS Status,
                    COUNT(*) AS Data
                FROM 
                    Contributions AS cs
                INNER JOIN 
                    AcademicYears AS ay ON cs.AcademicYearId = ay.Id
                WHERE 
                    cs.UserId = @currentUserId
                GROUP BY 
                    ay.Name, 
                    CASE cs.Status
                        WHEN 0 THEN 'Pending'
                        WHEN 1 THEN 'Approve'
                        WHEN 2 THEN 'Reject'
                    END
                ORDER BY 
                    ay.Name, 
                    CASE cs.Status
                        WHEN 0 THEN 'Pending'
                        WHEN 1 THEN 'Approve'
                        WHEN 2 THEN 'Reject'
                    END;";
        var items = await conn.QueryAsync<TotalContributionFollowingStatusData>(sql: sql,param: new{currentUserId});
        return await _contributionReportMapper.MapToTotalContributionFollowingStatusDataResponse(items.AsList());
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

    public async Task<ReportChartResponse<PercentageTotalContributionsPerFacultyPerAcademicYearData>> GetPercentageTotalContributionsPerFacultyPerAcademicYearReport(string academicYearName)
    {
        using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        if (conn.State == ConnectionState.Open)
        {
            await conn.OpenAsync();
        }

        var sql = @"
                SELECT ay.Name AS AcademicYear,
                    f.Name AS Faculty,
                    cast((count(c.Id) * 1.0 / total_count.total * 100) as int)  AS Percentages
                FROM AcademicYears ay
                CROSS JOIN Faculties f
                LEFT JOIN Contributions c ON c.AcademicYearId = ay.Id AND c.FacultyId = f.Id
                LEFT JOIN (
                    SELECT COUNT(*) AS total
                    FROM Contributions c2
                    left join AcademicYears ay2 on c2.AcademicYearId = ay2.Id
                    where ay2.Name = @academicYearName
                    group by ay2.Name
                ) AS total_count ON 1=1
                GROUP BY ay.Name, f.Name, total_count.total
                having ay.Name = @academicYearName
                ORDER BY ay.Name, f.Name;
            ";

        var items = await conn.QueryAsync<PercentagesContributionsWithinEachFacultyForEachAcademicYearDto>(sql: sql, param: new
        {
            academicYearName
        });

        return _contributionReportMapper.MapToPercentageTotalContributionsPerFacultyPerAcademicYearReportChartResponse(items.AsList());
    }

    public async Task<ReportChartResponse<TotalContributorsPerFacultyData>> GetTotalContributorsPerEachFacultiesPerEachAcademicYearsDto()
    {
        using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        if (conn.State == ConnectionState.Open)
        {
            await conn.OpenAsync();
        }

        var sql = @"
               SELECT ay.Name AS AcademicYear,
                    f.Name AS Faculty,	   	   
                    COALESCE(count(distinct c.UserId), 0) AS Contributors	   
                FROM AcademicYears ay
                CROSS JOIN Faculties f
                LEFT JOIN Contributions c ON c.AcademicYearId = ay.Id AND c.FacultyId = f.Id
                GROUP BY ay.Name, f.Name
                ORDER BY ay.Name, f.Name;
            ";

        var items = await conn.QueryAsync<TotalContributorsPerEachFacultiesPerEachAcademicYearsDto>(sql: sql);

        return await _contributionReportMapper.MapToTotalContributorsPerEachFacultiesPerEachAcademicYearsResponse(items.AsList());
    }
}