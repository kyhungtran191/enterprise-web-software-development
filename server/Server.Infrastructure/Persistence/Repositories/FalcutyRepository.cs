using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.Application.Common.Dtos.Faculties;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers.PagedResult;
using Server.Domain.Entity.Content;

namespace Server.Infrastructure.Persistence.Repositories;

public class FalcutyRepository : RepositoryBase<Faculty, Guid>, IFacultyRepository
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    public FalcutyRepository(AppDbContext context, IMapper mapper) : base(context)
    {
        _dbContext = context;
        _mapper = mapper;
    }

    public async Task<int> Count()
    {
        return await _dbContext.Faculties.CountAsync();
    }

    public async Task<PagedResult<FacultyDto>> GetAllFacultiesPaging(string? keyword, int pageIndex = 1, int pageSize = 10)
    {
        var query = _dbContext.Faculties.AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(x => x.Name.Contains(keyword));
        }

        var totalCount = await query.CountAsync();

        var skipPage = (pageIndex - 1 < 0 ? 1 : pageIndex - 1) * pageSize; ;

        query =
             query
            .Where(x => x.DateDeleted == null)
            .OrderByDescending(x => x.DateCreated)
            .Skip(skipPage)
            .Take(pageSize);

        return new PagedResult<FacultyDto>
        {
            CurrentPage = pageIndex,
            RowCount = totalCount,
            PageSize = pageSize,
            Results = await _mapper.ProjectTo<FacultyDto>(query).ToListAsync(),
        };
    }

    public async Task<Faculty> GetFacultyByName(string facultyName)
    {
        return await _dbContext.Faculties
                .SingleOrDefaultAsync(x => x.Name == facultyName);
    }
}