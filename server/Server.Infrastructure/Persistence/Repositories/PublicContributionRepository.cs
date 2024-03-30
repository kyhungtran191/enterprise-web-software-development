using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.Application.Common.Dtos;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers.PagedResult;
using Server.Domain.Common.Constants;
using Server.Domain.Entity.Content;
using System.Drawing.Printing;

namespace Server.Infrastructure.Persistence.Repositories
{
    public class PublicContributionRepository : RepositoryBase<ContributionPublic, Guid>, IPublicContributionRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        public PublicContributionRepository(AppDbContext dbContext,IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
                
        }
        public async Task<PagedResult<PublicContributionInListDto>> GetAllPaging(string? keyword, string? year, string? facultyName, string? status, int pageIndex = 1, int pageSize = 10)
        {
            var query = from c in _dbContext.ContributionPublics
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

            if (!string.IsNullOrEmpty(status))
            {

                if (Enum.TryParse<ContributionStatus>(status.ToUpperInvariant(), true, out var statusEnum))
                {
                    query = query.Where(x => x.c.Status == statusEnum);
                }
                else
                {
                    throw new Exception("Invalid Status");
                }


            }
            var totalRow = await query.CountAsync();

            var skipRow = (pageIndex - 1 < 0 ? 1 : pageIndex - 1) * pageIndex;
            var contributions = await query
                .OrderByDescending(x => x.c.DateCreated)
                .Skip(skipRow)
                .Take(pageSize)
                .ToListAsync();

            var contributionIds = contributions.Select(x => x.c.Id).ToList();
            var files = await _dbContext.Files
                .Where(f => contributionIds.Contains(f.ContributionId))
                .ToListAsync();
          
            var publicContribution = contributions.Select(x => new PublicContributionInListDto
            {
                Id = x.c.Id,
                Title = x.c.Title,
                UserName = x.u.FirstName,
                FacultyName = x.f.Name,
                AcademicYear = x.a.Name,
                PublicDate = x.c.PublicDate,
                Slug = x.c.Slug,
                DateEdited = x.c.DateEdited,
                Thumbnails = files.Where(f => f.ContributionId == x.c.Id && f.Type == FileType.Thumbnail)
                    .Select(f => new FileReturnDto { Path = f.Path, Name = f.Name }).ToList(),
                Like = x.c.LikeQuantity,
                View = x.c.Views
            }).ToList();
            return new PagedResult<PublicContributionInListDto>
            {
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize,
                Results =  publicContribution
            };
        }
        public async Task<List<PublicContributionInListDto>> GetTopContributions(int quantity = 4)
        {
            var query = from c in _dbContext.ContributionPublics
                where c.DateDeleted == null
                join u in _dbContext.Users on c.UserId equals u.Id
                join f in _dbContext.Faculties on c.FacultyId equals f.Id
                join a in _dbContext.AcademicYears on c.AcademicYearId equals a.Id
                select new { c, u, f, a };
            var contributions = await query
                .OrderBy(x => x.c.DateCreated)
                .Take(quantity)
                .ToListAsync();
            var contributionIds = contributions.Select(x => x.c.Id).ToList();
            var files = await _dbContext.Files
                .Where(f => contributionIds.Contains(f.ContributionId))
                .ToListAsync();
          
            var publicContribution = contributions.Select(x => new PublicContributionInListDto
            {
                Id = x.c.Id,
                Title = x.c.Title,
                UserName = x.u.FirstName,
                FacultyName = x.f.Name,
                AcademicYear = x.a.Name,
                PublicDate = x.c.PublicDate,
                Slug = x.c.Slug,
                DateEdited = x.c.DateEdited,
                Thumbnails = files.Where(f => f.ContributionId == x.c.Id && f.Type == FileType.Thumbnail)
                    .Select(f => new FileReturnDto { Path = f.Path, Name = f.Name }).ToList(),
                Like = x.c.LikeQuantity,
                View = x.c.Views
            }).ToList();
            return publicContribution;
        }
    }
    
}
