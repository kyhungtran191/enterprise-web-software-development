using Server.Application.Common.Interfaces.Persistence;
using Server.Domain.Entity.Content;

namespace Server.Infrastructure.Persistence.Repositories;

public class FalcutyRepository : RepositoryBase<Faculty, Guid>, IFacultyRepository
{
    public FalcutyRepository(AppDbContext context) : base(context)
    {
    }
}