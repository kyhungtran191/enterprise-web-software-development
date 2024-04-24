using Microsoft.AspNetCore.Mvc;

namespace Server.Contracts.Contributions
{
    public class RejectContributionRequest
    {      
      public Guid Id { get; set; }
      public string Note { get; set; }
    }
}
