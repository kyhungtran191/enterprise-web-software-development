﻿using System.Data;
using System.Drawing.Printing;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.Application.Common.Dtos;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Common.Dtos.Tags;
using Server.Application.Common.Extensions;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers.PagedResult;
using Server.Domain.Common.Constants;
using Server.Domain.Entity.Content;


namespace Server.Infrastructure.Persistence.Repositories
{
    public class ContributionRepository : RepositoryBase<Contribution, Guid>, IContributionRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public ContributionRepository(AppDbContext context,
                                      IMapper mapper) : base(context)
        {
            _dbContext = context;
            _mapper = mapper;
        }
        public async Task<bool> IsSlugAlreadyExisted(string slug, Guid? id = null)
        {
            if (id.HasValue)
            {
                return await _dbContext.Contributions.AnyAsync(c => c.Slug == slug && c.Id != id.Value);
            }
            return await _dbContext.Contributions.AnyAsync(c => c.Slug == slug);
        }
        public bool IsConfirmed(Guid contributionId)
        {
            var contribution = GetByIdAsync(contributionId).GetAwaiter().GetResult();
            return contribution.IsConfirmed;
        }
        public async Task<PagedResult<NotCommentContributionDto>> GetUncommentedContributions(int pageIndex = 1, int pageSize = 10)
        {
            var currentDate = DateTime.UtcNow;
            var query = from c in _dbContext.Contributions
                where c.DateDeleted == null && c.IsCoordinatorComment == false
                join u in _dbContext.Users on c.UserId equals u.Id
                join f in _dbContext.Faculties on c.FacultyId equals f.Id
                join a in _dbContext.AcademicYears on c.AcademicYearId equals a.Id
                select new { c, u, f, a };

            var totalRow = await query.CountAsync(); 
            var skipRow = (pageIndex - 1 < 0 ? 1 : pageIndex - 1) * pageSize;
            var contributions = await query
                .OrderByDescending(x => x.c.DateCreated)
                .Skip(skipRow)
                .Take(pageSize)
                .ToListAsync();
            var result = contributions.Select(x => new NotCommentContributionDto
            {
                Title = x.c.Title,
                Slug = x.c.Slug,
                UserName = x.u.UserName,
                AcademicYear = x.a.Name,
                FacultyName = x.f.Name,
                Id = x.c.Id,
                SubmissionDate = x.c.SubmissionDate,
                IsCoordinatorComment = x.c.IsCoordinatorComment

            }).ToList();


            return new PagedResult<NotCommentContributionDto>
            {
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize,
                Results = result
            };
        }
        public async Task<PagedResult<ContributionInListDto>> GetAllPaging(string? keyword, string? year, string? facultyName, Guid? userId, string? status, int pageIndex = 1, int pageSize = 10, bool? GuestAllowed = false)
        {
            var query = from c in _dbContext.Contributions
                        where c.DateDeleted == null
                        join u in _dbContext.Users on c.UserId equals u.Id
                        join f in _dbContext.Faculties on c.FacultyId equals f.Id
                        join a in _dbContext.AcademicYears on c.AcademicYearId equals a.Id
                        select new { c, u, f, a };

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => EF.Functions.Like(x.c.Title, $"%{keyword}%"));
            }

            if (!string.IsNullOrEmpty(year))
            {
                query = query.Where(x => x.a.Name == year);
            }

            if (!string.IsNullOrEmpty(facultyName))
            {
                query = query.Where(x => x.f.Name == facultyName);
            }

            if (userId.HasValue)
            {
                query = query.Where(x => x.c.UserId == userId);
            }

            if (!string.IsNullOrEmpty(status))
            {

                if (Enum.TryParse<ContributionStatus>(status.ToUpperInvariant(), true, out var statusEnum))
                {
                    query = query.Where(x => x.c.Status == statusEnum);
                    if (statusEnum == ContributionStatus.Approve)
                    {
                        query = query.OrderByDescending(x => x.c.PublicDate);
                    }
                    
                }
                else
                {
                    throw new Exception("Invalid Status");
                }


            }
            if (GuestAllowed is true)
            {
                query = query.Where(x => x.c.AllowedGuest == true);
            }
            var totalRow = await query.CountAsync();

            var skipRow = (pageIndex - 1 < 0 ? 1 : pageIndex - 1) * pageSize;
            var contributions = await query
                .OrderByDescending(x => x.c.DateCreated)
                .Skip(skipRow)
                .Take(pageSize)
                .ToListAsync();

            var contributionIds = contributions.Select(x => x.c.Id).ToList();
            var files = await _dbContext.Files
                .Where(f => contributionIds.Contains(f.ContributionId))
                .ToListAsync();
            // get average rating
            var publicContributions = await _dbContext.ContributionPublics.Where(x => contributionIds.Contains(x.Id))
                .ToListAsync();
            var contributionsDto = contributions.Select(x => new ContributionInListDto
            {
                Id = x.c.Id,
                Title = x.c.Title,
                UserName = x.u.UserName,
                FacultyName = x.f.Name,
                AcademicYear = x.a.Name,
                SubmissionDate = x.c.SubmissionDate,
                PublicDate = x.c.PublicDate,
                Slug = x.c.Slug,
                Status = x.c.Status.ToStringValue(),
                Thumbnails = files.Where(f => f.ContributionId == x.c.Id && f.Type == FileType.Thumbnail)
                    .Select(f => new FileReturnDto {Path = f.Path, Name = f.Name, Extension = f.Extension, PublicId = f.PublicId }).ToList(),
                Files = files.Where(f => f.ContributionId == x.c.Id && f.Type == FileType.File)
                    .Select(f => new FileReturnDto {Path = f.Path, Name = f.Name, Extension = f.Extension, PublicId = f.PublicId }).ToList(),
                RejectedReason = GetRejectReason(x.c).GetAwaiter().GetResult(),
                ShortDescription = x.c.ShortDescription,
                GuestAllowed = x.c.AllowedGuest,
                AverageRating = publicContributions.Where(p=>p.Id == x.c.Id).FirstOrDefault()?.AverageRating,
                Avatar = x.u.Avatar
            }).ToList();



            return new PagedResult<ContributionInListDto>
            {
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize,
                Results = contributionsDto
            };

        }

        public async Task<List<TagDto>> GetAllTags(Guid contributionId)
        {
            var query = from p in _dbContext.ContributionTags
                        join t in _dbContext.Tags
                            on p.TagId equals t.Id
                        where t.Id == contributionId
                        select t;
            return await _mapper.ProjectTo<TagDto>(query).ToListAsync();
        }
        public async Task<ContributionDto> GetContributionOfFaculty(string slug, string facultyName)
        {
            var query = from c in _dbContext.Contributions
                where c.Slug == slug && c.DateDeleted == null
                join u in _dbContext.Users on c.UserId equals u.Id
                join f in _dbContext.Faculties on c.FacultyId equals f.Id
                join a in _dbContext.AcademicYears on c.AcademicYearId equals a.Id
                where f.Name == facultyName
                select new
                {
                    c,
                    u,
                    f,
                    a
                };

          

            if (!string.IsNullOrEmpty(facultyName))
            {
                query = query.Where(x => x.f.Name == facultyName);
            }

            var contributionDetail = await query.FirstOrDefaultAsync();
            if (contributionDetail == null)
            {
                return null;
            }

            var files = await _dbContext.Files.Where(f => f.ContributionId == contributionDetail.c.Id).ToListAsync();
            var result = new ContributionDto
            {
                Id = contributionDetail.c.Id,
                Title = contributionDetail.c.Title,
                Slug = contributionDetail.c.Slug,
                Status = contributionDetail.c.Status.ToStringValue(),
                UserName = contributionDetail.u.UserName,
                FacultyName = contributionDetail.f.Name,
                AcademicYear = contributionDetail.a.Name,
                SubmissionDate = contributionDetail.c.SubmissionDate,
                PublicDate = contributionDetail.c.PublicDate,
                DateEdited = contributionDetail.c.DateEdited,
                Thumbnails = files.Where(f => f.ContributionId == contributionDetail.c.Id && f.Type == FileType.Thumbnail)
                    .Select(f => new FileReturnDto {Path = f.Path, Name = f.Name, Extension = f.Extension, PublicId = f.PublicId }).ToList(),
                Files = files.Where(f => f.ContributionId == contributionDetail.c.Id && f.Type == FileType.File)
                    .Select(f => new FileReturnDto { Path = f.Path, Name = f.Name, Extension = f.Extension, PublicId = f.PublicId }).ToList(),
                ShortDescription = contributionDetail.c.ShortDescription,
                Content = contributionDetail.c.Content
            };


            return result;
        }

        public async Task<ContributionDto> GetContributionOfUser(string slug, Guid userId)
        {
            var contributionDetail = await (from c in _dbContext.Contributions
                                            where c.Slug == slug && c.DateDeleted == null && c.UserId == userId
                                            join u in _dbContext.Users on c.UserId equals u.Id
                                            join f in _dbContext.Faculties on c.FacultyId equals f.Id
                                            join a in _dbContext.AcademicYears on c.AcademicYearId equals a.Id
                                            select new
                                            {
                                                c,
                                                u,
                                                f,
                                                a
                                            }).FirstOrDefaultAsync();


            if (contributionDetail == null)
            {
                return null;
            }

            var files = await _dbContext.Files.Where(f => f.ContributionId == contributionDetail.c.Id).ToListAsync();
            var result = new ContributionDto
            {
                Id = contributionDetail.c.Id,
                Title = contributionDetail.c.Title,
                Slug = contributionDetail.c.Slug,
                Status = contributionDetail.c.Status.ToStringValue(),
                UserName = contributionDetail.u.UserName,
                FacultyName = contributionDetail.f.Name,
                AcademicYear = contributionDetail.a.Name,
                SubmissionDate = contributionDetail.c.SubmissionDate,
                PublicDate = contributionDetail.c.PublicDate,
                DateEdited = contributionDetail.c.DateEdited,
                Thumbnails = files.Where(f => f.ContributionId == contributionDetail.c.Id && f.Type == FileType.Thumbnail)
                    .Select(f => new FileReturnDto { Path = f.Path, Name = f.Name, Extension = f.Extension, PublicId = f.PublicId}).ToList(),
                Files = files.Where(f => f.ContributionId == contributionDetail.c.Id && f.Type == FileType.File)
                    .Select(f => new FileReturnDto { Path = f.Path, Name = f.Name, Extension = f.Extension, PublicId = f.PublicId}).ToList(),
                ShortDescription = contributionDetail.c.ShortDescription,
                Content = contributionDetail.c.Content,
            };


            return result;
        }

        public async Task<ContributionDto> GetContributionBySlug(string slug)
        {
            var contributionDetail = await (from c in _dbContext.Contributions
                                            where c.Slug == slug && c.DateDeleted == null
                                            join u in _dbContext.Users on c.UserId equals u.Id
                                            join f in _dbContext.Faculties on c.FacultyId equals f.Id
                                            join a in _dbContext.AcademicYears on c.AcademicYearId equals a.Id
                                            select new
                                            {
                                                c,
                                                u,
                                                f,
                                                a
                                            }).FirstOrDefaultAsync();

            if (contributionDetail == null)
            {
                return null;
            }
            var files = await _dbContext.Files.Where(f => f.ContributionId == contributionDetail.c.Id).ToListAsync();


            var result = new ContributionDto
            {
                Id = contributionDetail.c.Id,
                Title = contributionDetail.c.Title,
                Slug = contributionDetail.c.Slug,
                Status = contributionDetail.c.Status.ToStringValue(),
                UserName = contributionDetail.u.UserName,
                FacultyName = contributionDetail.f.Name,
                AcademicYear = contributionDetail.a.Name,
                SubmissionDate = contributionDetail.c.SubmissionDate,
                PublicDate = contributionDetail.c.PublicDate,
                DateEdited = contributionDetail.c.DateEdited,
                Thumbnails = files.Where(f => f.ContributionId == contributionDetail.c.Id && f.Type == FileType.Thumbnail)
                    .Select(f => new FileReturnDto {Path = f.Path, Name = f.Name, Extension = f.Extension, PublicId = f.PublicId }).ToList(),
                Files = files.Where(f => f.ContributionId == contributionDetail.c.Id && f.Type == FileType.File)
                    .Select(f => new FileReturnDto { Path = f.Path, Name = f.Name, Extension = f.Extension, PublicId = f.PublicId }).ToList(),
                ShortDescription = contributionDetail.c.ShortDescription,
                Content = contributionDetail.c.Content,
            };

            return result;
        }



        public async Task SendToApprove(Guid contributionId, Guid userId)
        {
            var contribution = await _dbContext.Contributions.FindAsync(contributionId);
            if (contribution is null)
            {
                throw new Exception($"Not found contribution with id {contributionId}");
            }
            var user = await _dbContext.Users.FindAsync(userId);
            if (user is null)
            {
                throw new Exception($"Not found user");
            }

            await _dbContext.ContributionActivityLogs.AddAsync(new ContributionActivityLog
            {
                Id = Guid.NewGuid(),
                ContributionId = contribution.Id,
                Title = contribution.Title,
                UserId = userId,
                UserName = user?.UserName,
                FromStatus = contribution.Status,
                ToStatus = ContributionStatus.Pending,
                Description = $"{user.UserName} submit new contribution and waiting for approval"

            });
            contribution.Status = ContributionStatus.Pending;
            _dbContext.Contributions.Update(contribution);

        }
        public async Task Approve(Contribution contribution, Guid userId)
        {
            // who approve
            var user = await _dbContext.Users.FindAsync(userId);
            if (user is null)
            {
                throw new Exception($"Not found user");
            }
            var faculty = await _dbContext.Faculties.FindAsync(contribution.FacultyId);
            if (faculty is null)
            {
                throw new Exception("This contribution is error");
            }
            await _dbContext.ContributionActivityLogs.AddAsync(new ContributionActivityLog
            {
                Id = Guid.NewGuid(),
                ContributionId = contribution.Id,
                Title = contribution.Title,
                UserId = userId,
                UserName = user?.UserName,
                FromStatus = contribution.Status,
                ToStatus = ContributionStatus.Approve,
                Description = $"{user?.UserName} approve"

            });
            // who send contribution
            var contributionOwner = await _dbContext.Users.FindAsync(contribution.UserId);
            contribution.Status = ContributionStatus.Approve;
            contribution.PublicDate = DateTime.UtcNow;
            _dbContext.Contributions.Update(contribution);
            var publicContribution = _mapper.Map<ContributionPublic>(contribution);
            publicContribution.Id = contribution.Id;
            publicContribution.Avatar = contributionOwner.Avatar ?? String.Empty;
            publicContribution.UserName = contributionOwner.UserName;
            publicContribution.FacultyName = faculty.Name;
            publicContribution.DateCreated = DateTime.UtcNow;

            await _dbContext.ContributionPublics.AddAsync(publicContribution);

        }
        public async Task<string> GetRejectReason(Contribution contribution)
        {

            var activity = await _dbContext.ContributionActivityLogs
                .Where(pal => pal.ContributionId == contribution.Id && pal.ToStatus == ContributionStatus.Reject)
                .OrderByDescending(pal => pal.DateCreated)
                .FirstOrDefaultAsync();
            return activity?.Description ?? string.Empty;
        }


        public async Task Reject(Contribution contribution, Guid userId, string note)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user is null)
            {
                throw new Exception($"Not found user");
            }
            await _dbContext.ContributionActivityLogs.AddAsync(new ContributionActivityLog
            {
                Id = Guid.NewGuid(),
                ContributionId = contribution.Id,
                Title = contribution.Title,
                UserId = userId,
                UserName = user?.UserName,
                FromStatus = contribution.Status,
                ToStatus = ContributionStatus.Reject,
                Description = note

            });
            contribution.Status = ContributionStatus.Reject;
            _dbContext.Contributions.Update(contribution);
        }

        public async Task<List<ActivityLogDto>> GetActivityLogs(Contribution contribution)
        {
            var items = await _dbContext.ContributionActivityLogs.Where(x => x.ContributionId == contribution.Id).OrderByDescending(x => x.DateCreated).ToListAsync();
            var results = new List<ActivityLogDto>();
            foreach (var item in items)
            {
                var result = _mapper.Map<ActivityLogDto>(item);
                result.FromStatus = item.FromStatus.ToStringValue();
                result.ToStatus = item.ToStatus.ToStringValue();
                results.Add(result);

            }

            return results;
        }


    }
}
