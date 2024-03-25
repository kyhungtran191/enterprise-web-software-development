namespace Server.Application.Common.Dtos.Contributions
{
    public class ContributionDto : ContributionInListDto
    {
        public string Thumbnail { get; set; } = default!;
        public DateTime? DateEditted { get; set; }
    }
}
