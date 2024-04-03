using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.Application.Common.Dtos.AcademicYears;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers.PagedResult;
using Server.Domain.Entity.Content;

namespace Server.Infrastructure.Persistence.Repositories;

public class AcademicYearRepository : RepositoryBase<AcademicYear, Guid>, IAcademicYearRepository
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    public AcademicYearRepository(AppDbContext context, IMapper mapper) : base(context)
    {
        _dbContext = context;
        _mapper = mapper;
    }

    public async Task<AcademicYear> GetAcademicYearByName(string name)
    {
        return await _dbContext.AcademicYears.SingleOrDefaultAsync(x => x.Name == name);
    }
    public async Task<AcademicYear> GetAcademicYearByDateAsync(DateTime date)
    {
        var academicYear = await _dbContext.AcademicYears
            .Where(a => date >= a.StartClosureDate && date <= a.FinalClosureDate)
            .FirstOrDefaultAsync();

        return academicYear;
    }
    public async Task<bool> CanSubmitAsync(DateTime date)
    {
        var academicYear = await _dbContext.AcademicYears
            .Where(a => date >= a.StartClosureDate && date <= a.EndClosureDate)
            .FirstOrDefaultAsync();

        return academicYear != null;
    }
    public async Task<bool> CanEditAsync(DateTime date)
    {
        var academicYear = await _dbContext.AcademicYears
            .Where(a => date >= a.EndClosureDate && date <= a.FinalClosureDate)
            .FirstOrDefaultAsync();

        return academicYear != null;
    }

    public async Task<PagedResult<AcademicYearDto>> GetAllYearsPaging(string? keyword, int pageIndex = 1, int pageSize = 10)
    {
        var query = _dbContext.AcademicYears.AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(x => x.Name.Contains(keyword));
        }

        var totalCount = await query.CountAsync();

        var skipPage = (pageIndex - 1 < 0 ? 1 : pageIndex - 1) * pageSize; 

        query =
             query
            .Where(x => x.DateDeleted == null)
            .OrderByDescending(x => x.DateCreated)
            .Skip(skipPage)
            .Take(pageSize);
        var test = query.ToList();
        return new PagedResult<AcademicYearDto>
        {
            CurrentPage = pageIndex,
            RowCount = totalCount,
            PageSize = pageSize,
            Results = await _mapper.ProjectTo<AcademicYearDto>(query).ToListAsync(),
        };
    }

    public async Task<bool> HasContributionsAsync(Guid academicYearId)
    {
        return await _dbContext.Contributions.AnyAsync(c => c.AcademicYearId == academicYearId);
    }

    public async Task<bool> AnyActiveYear()
    {
        return await _dbContext.AcademicYears.AnyAsync(x => x.IsActive == true);
    }
}