using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.Application.Common.Dtos;
using Server.Application.Common.Interfaces.Persistence;
using Server.Domain.Entity.Content;
using File = Server.Domain.Entity.Content.File;

namespace Server.Infrastructure.Persistence.Repositories
{ 
    public class FilesRepository : RepositoryBase<File,Guid>, IFileRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public FilesRepository(AppDbContext context,IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<File>> GetByContribution(Contribution contribution)
        {
            var files = await _context.Files.Where(x => x.ContributionId == contribution.Id).ToListAsync();
            return files;
        }
    }
}
