namespace Server.Contracts.Contributions.report
{
    public class TotalContributionFollowingStatusData
    {
        public string? AcademicYear { get; set; }
        public int? TotalLike { get; set; }
        public int? TotalComment { get; set; }
        public int? TotalContributionApproved { get; set; }
        public float? AverageRating { get; set; }
        public int? Pending { get; set; }
        public int? Approve { get; set; }
        public int? Reject { get; set; }
        
    }
    public class TotalContributionFollowingStatusDataSet
    {
        public string? Status { get; set; }
        public int Data { get; set; }
    }


}
