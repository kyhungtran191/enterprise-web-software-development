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
                join a in _dbContext.AcademicYears on c.AcademicYearId equals a.Id
                select new { c, a };

           
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
                query = query.Where(x => x.c.FacultyName == facultyName);
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
                UserName = x.c.UserName,
                FacultyName = x.c.FacultyName,
                AcademicYear = x.a.Name,
                Avatar = x.c.Avatar,
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
                join a in _dbContext.AcademicYears on c.AcademicYearId equals a.Id
                select new { c, a };
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
                UserName = x.c.UserName,
                FacultyName = x.c.FacultyName,
                AcademicYear = x.a.Name,
                Avatar = x.c.Avatar,
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

        public async Task AddToReadLater(ContributionPublic contribution,Guid userId)
        {
            await  _dbContext.ContributionPublicReadLaters.AddAsync(new ContributionPublicReadLater
                { ContributionPublicId = contribution.Id, UserId = userId, DateCreated = DateTime.UtcNow });
        }

        public async Task AddToFavorite(ContributionPublic contribution, Guid userId)
        {
            await _dbContext.ContributionPublicFavorites.AddAsync(new ContributionPublicFavorite
                { ContributionPublicId = contribution.Id, UserId = userId, DateCreated = DateTime.UtcNow });
        }

        public async Task<bool> AlreadyReadLater(ContributionPublic contribution, Guid userId)
        {
            return await _dbContext.ContributionPublicReadLaters.AnyAsync(x =>
                x.ContributionPublicId == contribution.Id && x.UserId == userId);
        }

        public async Task<ContributionPublicReadLater> GetReadLater(ContributionPublic contribution, Guid userId)
        {
            return await _dbContext.ContributionPublicReadLaters.Where(x =>
                x.ContributionPublicId == contribution.Id && x.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<bool> AlreadyFavorite(ContributionPublic contribution, Guid userId)
        {
            return await _dbContext.ContributionPublicFavorites.AnyAsync(x =>
                x.ContributionPublicId == contribution.Id && x.UserId == userId);
        }

        public async Task<ContributionPublicFavorite> GetFavorite(ContributionPublic contribution, Guid userId)
        {
            return await _dbContext.ContributionPublicFavorites.Where(x =>
                x.ContributionPublicId == contribution.Id && x.UserId == userId).SingleOrDefaultAsync();
        }

        public void RemoveReadLater(ContributionPublicReadLater readLater)
        {
            _dbContext.ContributionPublicReadLaters.Remove(readLater);
        }

        public void RemoveFavorite(ContributionPublicFavorite favorite)
        {
            _dbContext.ContributionPublicFavorites.Remove(favorite);
        }
        public async Task<List<PublicContributionInListDto>> GetUserFavoriteContributions(Guid userId)
        {
            
            var query = from f in _dbContext.ContributionPublicFavorites
                where f.UserId == userId
                join c in _dbContext.ContributionPublics on f.ContributionPublicId equals c.Id
                where c.DateDeleted == null
                join a in _dbContext.AcademicYears on c.AcademicYearId equals a.Id
                select new { c, a };

            var favoriteContributions = await query
                .OrderBy(x => x.c.DateCreated)
                .ToListAsync();

            var contributionIds = favoriteContributions.Select(x => x.c.Id).ToList();

            var files = await _dbContext.Files
                .Where(f => contributionIds.Contains(f.ContributionId))
                .ToListAsync();

            var publicContribution = favoriteContributions.Select(x => new PublicContributionInListDto
            {
                Id = x.c.Id,
                Title = x.c.Title,
                UserName = x.c.UserName,
                FacultyName = x.c.FacultyName,
                AcademicYear = x.a.Name,
                Avatar = x.c.Avatar,
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
        public async Task<List<PublicContributionInListDto>> GetUserReadLaterContributions(Guid userId)
        {

            var query = from f in _dbContext.ContributionPublicReadLaters
                where f.UserId == userId
                join c in _dbContext.ContributionPublics on f.ContributionPublicId equals c.Id
                where c.DateDeleted == null
                join a in _dbContext.AcademicYears on c.AcademicYearId equals a.Id
                select new { c, a };

            var readlaterContributions = await query
                .OrderBy(x => x.c.DateCreated)
                .ToListAsync();

            var contributionIds = readlaterContributions.Select(x => x.c.Id).ToList();

            var files = await _dbContext.Files
                .Where(f => contributionIds.Contains(f.ContributionId))
                .ToListAsync();

            var publicContribution = readlaterContributions.Select(x => new PublicContributionInListDto
            {
                Id = x.c.Id,
                Title = x.c.Title,
                UserName = x.c.UserName,
                FacultyName = x.c.FacultyName,
                AcademicYear = x.a.Name,
                Avatar = x.c.Avatar,
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
