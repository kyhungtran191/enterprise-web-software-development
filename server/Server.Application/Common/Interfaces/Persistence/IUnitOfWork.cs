namespace Server.Application.Common.Interfaces.Persistence;

public interface IUnitOfWork : IDisposable
{
    Task<int> CompleteAsync();
    // IRepository<T, TKey> GetRepository<T, TKey>() where T : class;
    IFacultyRepository FacultyRepository { get; }
    IPrivateChatRepository PrivateChatRepository { get; }
    IPrivateMessagesRepository PrivateMessagesRepository { get; }
    ITagRepository TagRepository { get; }
    IAcademicYearRepository AcademicYearRepository { get; }
    IContributionRepository ContributionRepository { get; }
    IFileRepository FileRepository { get; }
    IPublicContributionRepository PublicContributionRepository { get; }
    ILikeRepository LikeRepository { get; }
    ICommentRepository CommentRepository { get; }
    IPublicCommentRepository PublicCommentRepository { get; }
    IRatingRepository RatingRepository { get; }

    IAnnouncementRepository AnnouncementRepository { get; }
    IAnnouncementUserRepository AnnouncementUserRepository{ get; }
}

