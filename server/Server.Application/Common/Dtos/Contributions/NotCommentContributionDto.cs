using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Common.Dtos.Contributions
{
    public class NotCommentContributionDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Slug { get; set; }
        public string UserName { get; set; }
        public string FacultyName { get; set; }
        public string AcademicYear { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public bool? IsCoordinatorComment { get; set; }

    }
}
