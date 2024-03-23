namespace Server.Contracts.Tags
{
    public class UpdateTagRequest
    {
        public Guid TagId { get; set; }
        public string TagName { get; set; } = null!;
    }
}
