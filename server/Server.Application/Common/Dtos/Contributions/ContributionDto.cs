namespace Server.Application.Common.Dtos.Contributions
{
    public class ContributionDto : ContributionInListDto
    {
        public DateTime? DateEdited { get; set; }
        public string? Content { get; set; }
    }
}
