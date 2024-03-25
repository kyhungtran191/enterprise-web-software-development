namespace Server.Application.Common.Interfaces.Persistence;

public interface IUnitOfWork : IDisposable
{
    Task<int> CompleteAsync();
    // IRepository<T, TKey> GetRepository<T, TKey>() where T : class;
    IFacultyRepository FacultyRepository { get; }
    ITagRepository TagRepository { get; }
    IAcademicYearRepository AcademicYearRepository { get; }
    IContributionRepository ContributionRepository { get; }
}

