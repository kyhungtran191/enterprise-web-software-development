namespace Server.Contracts.Contributions
{
    public class DeleteContributionRequest
    {
        public List<Guid> ContributionIds { get; set; } = default!;
    }
}
