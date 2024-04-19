using Microsoft.AspNetCore.Http;
using Server.Application.Common.Extensions;
using Server.Application.Common.Interfaces.Services;

namespace Server.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public Guid UserId =>
            _httpContextAccessor
                .HttpContext?
                .User
                .GetUserId() ??
            throw new ApplicationException("Invalid User Login");

        public bool IsAuthenticated =>
            _httpContextAccessor
                .HttpContext?
                .User
                .Identity?
                .IsAuthenticated ??
            throw new ApplicationException("Invalid User Login");
    }
}
