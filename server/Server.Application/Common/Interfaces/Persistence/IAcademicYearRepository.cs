using Server.Application.Common.Dtos.AcademicYears;
using Server.Application.Wrappers.PagedResult;
using Server.Domain.Entity.Content;

namespace Server.Application.Common.Interfaces.Persistence
{
    public interface IAcademicYearRepository : IRepository<AcademicYear,Guid>
    {
        Task<AcademicYear> GetAcademicYearByName(string name);
        Task<PagedResult<AcademicYearDto>> GetAllYearsPaging(string? keyword, int pageIndex = 1, int pageSize = 10);
    }
}
