namespace Server.Application.Common.Dtos.Contributions
{
    public class TopContributorDto
    {
        public string UserName { get; set; }
        public int TotalLikes { get; set; }
        public int ContributionCount { get; set; }
        public string Avatar { get; set; }
    }
}
