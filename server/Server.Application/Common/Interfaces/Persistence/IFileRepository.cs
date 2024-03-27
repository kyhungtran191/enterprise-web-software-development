using Server.Domain.Entity.Content;
using File = Server.Domain.Entity.Content.File;

namespace Server.Application.Common.Interfaces.Persistence
{
    public interface IFileRepository : IRepository<File,Guid>
    {
        Task<List<File>> GetByContribution(Contribution contribution);
    }
}
