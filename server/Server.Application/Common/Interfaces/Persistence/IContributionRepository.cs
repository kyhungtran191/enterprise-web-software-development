﻿using Server.Application.Common.Dtos.Contributions;
using Server.Application.Common.Dtos.Tags;
using Server.Application.Wrappers.PagedResult;
using Server.Domain.Entity.Content;

namespace Server.Application.Common.Interfaces.Persistence
{
    public interface IContributionRepository : IRepository<Contribution,Guid>
    {
        Task<PagedResult<ContributionInListDto>> GetAllPaging(string? keyword, string? year, string? facultyName, Guid? userId, string? status, int pageIndex = 1, int pageSize = 10,bool? GuestAllowed = false);
        bool IsConfirmed(Guid contributionId);
        Task<ContributionDto> GetContributionBySlug(string slug);
        Task<ContributionDto> GetContributionOfFaculty(string slug, string facultyName);
        Task<ContributionDto> GetContributionOfUser(string slug, Guid userId);

        Task<List<TagDto>> GetAllTags(Guid contributionId);
        
        Task<bool> IsSlugAlreadyExisted(string slug, Guid? id = null);
        Task Approve(Contribution contribution, Guid userId);
        Task SendToApprove(Guid contributionId,Guid userId);
        Task Reject(Contribution contribution,Guid userId, string note);
        Task<string> GetRejectReason(Contribution contribution);
        Task<List<ActivityLogDto>> GetActivityLogs(Contribution contribution);
        Task<PagedResult<NotCommentContributionDto>> GetUncommentedContributions(int pageIndex = 1, int pageSize = 10);

    }
}
