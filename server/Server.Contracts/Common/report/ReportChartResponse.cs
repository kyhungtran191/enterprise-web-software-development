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