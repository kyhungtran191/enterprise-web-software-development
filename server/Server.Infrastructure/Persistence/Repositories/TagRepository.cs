using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.Application.Common.Dtos.Faculties;
using Server.Application.Common.Dtos.Tags;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Wrappers.PagedResult;
using Server.Domain.Entity.Content;

namespace Server.Infrastructure.Persistence.Repositories;

public class TagRepository : RepositoryBase<Tag, Guid>, ITagRepository
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    public TagRepository(AppDbContext context, IMapper mapper) : base(context)
    {
        _dbContext = context;
        _mapper = mapper;
    }

    public async Task<PagedResult<TagDto>> GetAllTagsPaging(string? keyword, int pageIndex = 1, int pageSize = 10)
    {
        var query = _dbContext.Tags.AsQueryable();

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

        return new PagedResult<TagDto>
        {
            CurrentPage = pageIndex,
            RowCount = totalCount,
            PageSize = pageSize,
            Results = await _mapper.ProjectTo<TagDto>(query).ToListAsync(),
        };
    }

    public async Task<Tag> GetTagByName(string name)
    {
       return await _dbContext.Tags.SingleOrDefaultAsync(x=>x.Name==name);
    }
}