namespace Server.Application.Common.Interfaces.Services
{
    public interface IUserService
    {
        bool IsAuthenticated { get; }

        Guid UserId { get; }
        Guid FacultyId { get; }

    }
}
