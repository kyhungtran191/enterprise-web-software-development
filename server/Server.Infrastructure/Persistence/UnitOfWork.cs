using AutoMapper;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Common.Interfaces.Services;
using Server.Infrastructure.Persistence.Repositories;

namespace Server.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
  private readonly AppDbContext _context;
  private readonly IMapper _mapper;
  private readonly IUserService _userService;
    // private Dictionary<Type, object> _repositories;

    public UnitOfWork(AppDbContext context, IMapper mapper,IUserService userService)
  {
    _context = context;
    _mapper = mapper;
    _userService = userService;
  }

  public IFacultyRepository FacultyRepository => new FalcutyRepository(_context, _mapper);
  public ITagRepository TagRepository => new TagRepository(_context, _mapper);
  public IAcademicYearRepository AcademicYearRepository => new AcademicYearRepository(_context, _mapper);
  public IContributionRepository ContributionRepository => new ContributionRepository(_context, _mapper);
  public IFileRepository FileRepository => new FilesRepository(_context,_mapper);
  public IPublicContributionRepository PublicContributionRepository => new PublicContributionRepository(_context,_mapper,_userService);
  public ILikeRepository LikeRepository => new LikeRepository(_context);
  public ICommentRepository CommentRepository => new CommentRepository(_context);
  public IPublicCommentRepository PublicCommentRepository => new PublicCommentRepository(_context);
  public IRatingRepository RatingRepository => new RatingRepository(_context);
  public IAnnouncementRepository AnnouncementRepository => new AnnouncementRepository(_context);
  public IAnnouncementUserRepository AnnouncementUserRepository => new AnnouncementUserRepository(_context);
  public async Task<int> CompleteAsync()
  => await _context.SaveChangesAsync();

  public void Dispose()
      => _context.Dispose();


  // public IRepository<T, Key> GetRepository<T, Key>() where T : class
  // {
  //   _repositories ??= new Dictionary<Type, object>();

  //   var type = typeof(T);

  //   if (!_repositories.ContainsKey(type))
  //   {
  //     _repositories[type] = new RepositoryBase<T, Key>(_context);
  //   }

  //   return (IRepository<T, Key>)_repositories[type];
  // }
}