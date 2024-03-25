using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Common.Dtos.Tags;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers.PagedResult;
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
        public async Task<PagedResult<ContributionInListDto>> GetAllPaging(string? keyword, Guid? yearId, int pageIndex = 1, int pageSize = 10)
        {
            var query = from c in _dbContext.Contributions
                join u in _dbContext.Users on c.UserId equals u.Id
                join f in _dbContext.Faculties on c.FacultyId equals f.Id
                join a in _dbContext.AcademicYears on c.AcademicYearId equals a.Id
                where (c.DateDeleted == null)
                select new { c, u, f, a };

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.c.Title.Contains(keyword));
            }

            if (yearId.HasValue)
            {
                query = query.Where(x => x.c.AcademicYearId == yearId);
            }
            var totalRow = await query.CountAsync();

            var skipRow = (pageIndex - 1 < 0 ? 1 : pageIndex - 1) * pageIndex;

            var contributions = await query
                .OrderByDescending(x => x.c.DateCreated)
                .Skip(skipRow)
                .Take(pageSize)
                .Select(x => new ContributionInListDto
                {
                    
                   
                    Title = x.c.Title,
                    UserName = x.u.FirstName, 
                    FacultyName = x.f.Name, 
                    AcademicYear = x.a.Name, 
                    SubmissionDate = x.c.SubmissionDate
                   
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

        public async Task<ContributionDto>GetContributionByTitle(string title)
        {
            var contribution = await _dbContext.Contributions.SingleOrDefaultAsync(x => x.Title== title);
            return _mapper.Map<ContributionDto>(contribution);

        }
    }
}
