using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Server.Application.Common.Interfaces.Services;

namespace Server.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    public string UserId { get; }

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        UserId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}