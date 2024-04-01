namespace Server.Application.Common.Dtos.Contributions
{
    public class PublicContributionDetailDto : PublicContributionInListDto
    {
        public List<FileReturnDto>? Files { get; set; }
    }
}
