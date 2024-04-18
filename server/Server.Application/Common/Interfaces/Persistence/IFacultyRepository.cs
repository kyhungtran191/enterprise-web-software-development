using Server.Application.Common.Dtos.Faculties;
using Server.Application.Wrappers.PagedResult;
using Server.Domain.Entity.Content;

namespace Server.Application.Common.Interfaces.Persistence;

public interface IFacultyRepository : IRepository<Faculty, Guid>
{
    Task<Faculty> GetFacultyByName(string facultyName);

    Task<PagedResult<FacultyDto>> GetAllFacultiesPaging(string? keyword, int pageIndex = 1, int pageSize = 10);    

    Task<int> Count();
}