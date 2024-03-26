using Server.Application.Common.Interfaces.Persistence;
using File = Server.Domain.Entity.Content.File;

namespace Server.Infrastructure.Persistence.Repositories
{ 
    public class FilesRepository : RepositoryBase<File,Guid>, IFileRepository
    {
        private readonly AppDbContext _context;
        public FilesRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
