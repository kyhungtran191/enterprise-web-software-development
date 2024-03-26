using File = Server.Domain.Entity.Content.File;

namespace Server.Application.Common.Interfaces.Persistence
{
    public interface IFileRepository : IRepository<File,Guid>
    {
    }
}
