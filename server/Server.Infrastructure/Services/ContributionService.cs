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

    public async Task<CombineChartResponse<TotalContributionFollowingStatusDataSet>> GetContributionsFollowingStatusForEachAcademicYearOfCurrentUserReport(Guid currentUserId)
    {
        using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        if (conn.State == ConnectionState.Open)
        {
            await conn.OpenAsync();
        }

        var sql = @"WITH StatusData AS (
                        SELECT
                            ay.Name AS AcademicYear,
                            cs.Status,
                            COUNT(*) AS Data
                        FROM
                            Contributions AS cs
                        INNER JOIN
                            AcademicYears AS ay ON cs.AcademicYearId = ay.Id
                        WHERE
                            cs.UserId = @currentUserId AND cs.DateDeleted IS NULL
                        GROUP BY
                            ay.Name, cs.Status
                    ), InteractionData AS (
                        SELECT
                            ay.Name AS AcademicYear,
                            COALESCE(SUM(cp.Views), 0) AS TotalView,
                            COALESCE(COUNT(DISTINCT l.Id), 0) AS TotalLike,
                            COALESCE(CAST(AVG(cpr.Rating) AS DECIMAL(10, 2)), 0) AS AverageRating,
                            COALESCE(COUNT(DISTINCT cpc.Id), 0) AS TotalComment
                        FROM
                            AcademicYears ay
                        LEFT JOIN
                            ContributionPublics cp ON ay.Id = cp.AcademicYearId
                        LEFT JOIN
                            Likes l ON cp.Id = l.ContributionPublicId
                        LEFT JOIN
                            ContributionPublicRatings cpr ON cp.Id = cpr.ContributionPublicId
                        LEFT JOIN
                            ContributionPublicComments cpc ON cp.Id = cpc.ContributionId
                        WHERE
                            cp.UserId = @currentUserId AND cp.DateDeleted IS NULL
                        GROUP BY
                            ay.Name
                    )
                    SELECT
                        i.AcademicYear,
                        i.TotalLike,
                        i.TotalComment,
                        COALESCE(s1.Data, 0) AS TotalContributionApproved,
                        i.AverageRating,
                        COALESCE(s0.Data, 0) AS Pending,
                        COALESCE(s1.Data, 0) AS Approve,
                        COALESCE(s2.Data, 0) AS Reject
                    FROM
                        InteractionData i
                    LEFT JOIN
                        StatusData s0 ON i.AcademicYear = s0.AcademicYear AND s0.Status = 0
                    LEFT JOIN
                        StatusData s1 ON i.AcademicYear = s1.AcademicYear AND s1.Status = 1
                    LEFT JOIN
                        StatusData s2 ON i.AcademicYear = s2.AcademicYear AND s2.Status = 2
                    ORDER BY
                        i.AcademicYear;";
        var items = await conn.QueryAsync<TotalContributionFollowingStatusData>(sql: sql, param: new { currentUserId });
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
                WHERE c.DateDeleted is null
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
                SELECT 
                    ay.Name AS AcademicYear,
                    f.Name AS Faculty,
                    cast((count(c.Id) * 1.0 / total_count.total * 100) as int) AS Percentages
                FROM AcademicYears ay
                CROSS JOIN Faculties f
                LEFT JOIN Contributions c ON c.AcademicYearId = ay.Id 
                                            AND c.FacultyId = f.Id 
                                            AND c.DateDeleted is null
                                            AND f.DateDeleted is null							
                LEFT JOIN (
                    SELECT COUNT(*) AS total
                    FROM Contributions c2
                    left join AcademicYears ay2 on c2.AcademicYearId = ay2.Id
                    left join Faculties f2 on c2.FacultyId = f2.Id
                    where ay2.Name = @academicYearName
                    and c2.DateDeleted is null
                    and f2.DateDeleted is null
                    group by ay2.Name
                ) AS total_count ON 1=1
                where ay.Name = @academicYearName
                    and f.DateDeleted is null 
                    and c.DateDeleted is null
                GROUP BY ay.Name, f.Name, total_count.total
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
                WHERE f.DateDeleted is null
                GROUP BY ay.Name, f.Name
                ORDER BY ay.Name, f.Name;
            ";

        var items = await conn.QueryAsync<TotalContributorsPerEachFacultiesPerEachAcademicYearsDto>(sql: sql);

        return await _contributionReportMapper.MapToTotalContributorsPerEachFacultiesPerEachAcademicYearsResponse(items.AsList());
    }
}