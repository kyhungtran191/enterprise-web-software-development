namespace Server.Contracts.Common.report;

public class ReportChartResponse<T> where T : class, new() {
     public List<ReportChartResponseWrapper<T>> Response { get; set; }
    = new List<ReportChartResponseWrapper<T>>();
}

public class ReportChartResponseWrapper<T> where T : class, new()
{
    public String? AcademicYear { get; set; }

    public List<T> DataSets { get; set; } = new List<T>();
}
public class CombineChartResponse<T> where T : class, new()
{
    public List<CombineChartResponseWrapper<T>> Response { get; set; }
        = new List<CombineChartResponseWrapper<T>>();
}
public class CombineChartResponseWrapper<T> where T : class, new()
{
    public String? AcademicYear { get; set; }
    public int? TotalLike { get; set; }
    public int? TotalView { get; set; }
    public int? TotalComment { get; set; }
    public int? TotalContributionApproved { get; set; }
    public float? AverageRating { get; set; }

    public List<T> DataSets { get; set; } = new List<T>();
}