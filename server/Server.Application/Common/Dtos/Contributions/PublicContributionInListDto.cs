﻿using Server.Application.Common.Extensions;
using Server.Domain.Entity.Content;

namespace Server.Application.Common.Dtos.Contributions
{
    public class PublicContributionInListDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Slug { get; set; }
        public List<FileReturnDto> Thumbnails { get; set; }
        public string UserName { get; set; }
        public string FacultyName { get; set; }
        public string AcademicYear { get; set; }
        public DateTime? PublicDate { get; set; }
        public DateTime? DateEdited { get; set; }
        public int Like { get; set; } = 0;
        public int View { get; set; } = 0;
        public string? Avatar { get; set; }
        public string? ShortDescription { get; set; }
        public double AverageRating { get; set; } = 0.0;
        public double MyRating { get; set; } = 0.0;
        public string? WhoApproved { get; set; }

        public string? Status { get; set; } = ContributionStatus.Approve.ToStringValue();
    }
}
