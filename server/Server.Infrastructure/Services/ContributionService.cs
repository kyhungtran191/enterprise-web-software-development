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

        var sql = @"WITH ViewsData AS (
                        SELECT
                            cp.AcademicYearId,
                            SUM(cp.Views) AS TotalView
                        FROM
                            ContributionPublics cp
                        WHERE
                            cp.UserId = @currentUserId AND cp.DateDeleted IS NULL
                        GROUP BY
                            cp.AcademicYearId
                    ),
                    LikesData AS (
                        SELECT
                            cp.AcademicYearId,
                            COUNT(DISTINCT l.Id) AS TotalLike
                        FROM
                            ContributionPublics cp
                        LEFT JOIN
                            Likes l ON cp.Id = l.ContributionPublicId
                        WHERE
                            cp.UserId = @currentUserId AND cp.DateDeleted IS NULL
                        GROUP BY
                            cp.AcademicYearId
                    ),
                    RatingsData AS (
                        SELECT
                            cp.AcademicYearId,
                            CAST(AVG(cpr.Rating) AS DECIMAL(10, 2)) AS AverageRating
                        FROM
                            ContributionPublics cp
                        LEFT JOIN
                            ContributionPublicRatings cpr ON cp.Id = cpr.ContributionPublicId
                        WHERE
                            cp.UserId = @currentUserId AND cp.DateDeleted IS NULL
                        GROUP BY
                            cp.AcademicYearId
                    ),
                    CommentsData AS (
                        SELECT
                            cp.AcademicYearId,
                            COUNT(DISTINCT cpc.Id) AS TotalComment
                        FROM
                            ContributionPublics cp
                        LEFT JOIN
                            ContributionPublicComments cpc ON cp.Id = cpc.ContributionId
                        WHERE
                            cp.UserId = @currentUserId AND cp.DateDeleted IS NULL
                        GROUP BY
                            cp.AcademicYearId
                    ),
                    StatusData AS (
                        SELECT
                            ay.Id AS AcademicYearId,
                            ay.Name AS AcademicYear,
                            cs.Status,
                            COUNT(cs.Id) AS Data
                        FROM
                            Contributions cs
                        JOIN
                            AcademicYears ay ON cs.AcademicYearId = ay.Id
                        WHERE
                            cs.UserId = @currentUserId AND cs.DateDeleted IS NULL
                        GROUP BY
                            ay.Id, ay.Name, cs.Status
                    )
                    SELECT
                        ay.Name AS AcademicYear,
                        COALESCE(vd.TotalView, 0) AS TotalView,
                        COALESCE(ld.TotalLike, 0) AS TotalLike,
                        COALESCE(rd.AverageRating, 0) AS AverageRating,
                        COALESCE(cd.TotalComment, 0) AS TotalComment,
                        COALESCE(sd1.Data, 0) AS TotalContributionApproved,
                        COALESCE(sd0.Data, 0) AS Pending,
                        COALESCE(sd1.Data, 0) AS Approve,
                        COALESCE(sd2.Data, 0) AS Reject
                    FROM
                        AcademicYears ay
                    LEFT JOIN
                        ViewsData vd ON ay.Id = vd.AcademicYearId
                    LEFT JOIN
                        LikesData ld ON ay.Id = ld.AcademicYearId
                    LEFT JOIN
                        RatingsData rd ON ay.Id = rd.AcademicYearId
                    LEFT JOIN
                        CommentsData cd ON ay.Id = cd.AcademicYearId
                    LEFT JOIN
                        StatusData sd0 ON ay.Id = sd0.AcademicYearId AND sd0.Status = 0
                    LEFT JOIN
                        StatusData sd1 ON ay.Id = sd1.AcademicYearId AND sd1.Status = 1
                    LEFT JOIN
                        StatusData sd2 ON ay.Id = sd2.AcademicYearId AND sd2.Status = 2
                    ORDER BY
                        ay.Name;";
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