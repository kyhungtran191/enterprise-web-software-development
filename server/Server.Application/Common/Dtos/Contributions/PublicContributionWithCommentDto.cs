using Server.Application.Common.Dtos.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Common.Dtos.Contributions
{
    public class PublicContributionWithCommentDto : PublicContributionDetailDto
    {
        public List<CommentDto> Comments { get; set; }
    }
}
