using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contracts.Contributions
{
    public class GetTagsContributionRequest
    {
        public Guid ContributionId { get; set; }
    }
}
