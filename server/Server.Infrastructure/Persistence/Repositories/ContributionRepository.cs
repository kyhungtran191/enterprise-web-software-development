using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Common.Dtos.Tags;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers.PagedResult;
using Server.Domain.Common.Constants;
using Server.Domain.Entity.Content;

namespace Server.Infrastructure.Persistence.Repositories
{
    internal class ContributionRepository : RepositoryBase<Contribution, Guid>, IContributionRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public ContributionRepository(AppDbContext context , IMapper mapper) : base(context)
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
            return await _dbContext.Contributions.AnyAsync(c=>c.Slug == slug);
        }

        public async Task<PagedResult<ContributionInListDto>> GetAllPaging(string? keyword, Guid? yearId, int pageIndex = 1, int pageSize = 10)
        {
            var query = from c in _dbContext.Contributions
                join u in _dbContext.Users on c.UserId equals u.Id
                join f in _dbContext.Faculties on c.FacultyId equals f.Id
                join a in _dbContext.AcademicYears on c.AcademicYearId equals a.Id
                where (c.DateDeleted == null)
                select new
                {
                    Contribution = c,
                    User = u,
                    Faculty = f,
                    AcademicYear = a,
                    Files = _dbContext.Files.Where(file => file.ContributionId == c.Id).ToList()
                };

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Contribution.Title.Contains(keyword));
            }

            if (yearId.HasValue)
            {
                query = query.Where(x => x.Contribution.AcademicYearId == yearId);
            }
            var totalRow = await query.CountAsync();

            var skipRow = (pageIndex - 1 < 0 ? 1 : pageIndex - 1) * pageIndex;

            var contributions = await query
                .OrderByDescending(x => x.Contribution.DateCreated)
                .Skip(skipRow)
                .Take(pageSize)
                .Select(x => new ContributionInListDto
                {
                    
                   
                    Title = x.Contribution.Title,
                    UserName = x.User.FirstName, 
                    FacultyName = x.Faculty.Name, 
                    AcademicYear = x.AcademicYear.Name, 
                    SubmissionDate = x.Contribution.SubmissionDate,
                    Slug = x.Contribution.Slug,
                    ThumbnailUrl = x.Files.Where(f => f.Type == FileType.Thumbnail).Select(f => f.Path).ToList(),
                    FilePath = x.Files.Where(f => f.Type == FileType.File).Select(f => f.Path).ToList(),
                    Status = x.Contribution.Status,
                    
                })
                .ToListAsync();



            return new PagedResult<ContributionInListDto>
            {
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize,
                Results = contributions
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

        public async Task<ContributionDto> GetContributionBySlug(string slug)
        {
            var contributionDetail = await (from c in _dbContext.Contributions
                                            where c.Slug == slug && c.DateDeleted == null
                                            join u in _dbContext.Users on c.UserId equals u.Id
                                            join f in _dbContext.Faculties on c.FacultyId equals f.Id
                                            join a in _dbContext.AcademicYears on c.AcademicYearId equals a.Id
                                            select new
                                            {
                                                Contribution = c,
                                                User = u,
                                                Faculty = f,
                                                AcademicYear = a,
                                                Files = _dbContext.Files.Where(file => file.ContributionId == c.Id).ToList()
                                            }).FirstOrDefaultAsync();

            if (contributionDetail == null)
            {
                return null; 
            }

            
            var result = new ContributionDto
            {
                Title = contributionDetail.Contribution.Title,
                Slug = contributionDetail.Contribution.Slug,
                Status = contributionDetail.Contribution.Status,
                UserName = contributionDetail.User.FirstName,
                FacultyName = contributionDetail.Faculty.Name,
                AcademicYear = contributionDetail.AcademicYear.Name,
                SubmissionDate = contributionDetail.Contribution.SubmissionDate,
                PublicDate = contributionDetail.Contribution.PublicDate,
                DateEdited = contributionDetail.Contribution.DateEdited,
                ThumbnailUrl = contributionDetail.Files.Where(f => f.Type == FileType.Thumbnail).Select(f => f.Path).ToList(),
                FilePath = contributionDetail.Files.Where(f => f.Type == FileType.File).Select(f => f.Path).ToList()
            };

            return result;
        }




        public Task<List<ContributionDto>> GetPopularContributionAsync(int count)
        {
            throw new NotImplementedException();
        }
        public Task SendToApprove(Guid contributionId, Guid userId)
        {
            throw new NotImplementedException();
        }
        public Task Approve(Guid contributionId, Guid userId)
        {
            throw new NotImplementedException();
        }
        public Task<string> GetRejectReason(Guid contributionId)
        {
            throw new NotImplementedException();
        }

      
        public Task Reject(Guid contributionId, Guid userId, string note)
        {
            throw new NotImplementedException();
        }

      
    }
}
