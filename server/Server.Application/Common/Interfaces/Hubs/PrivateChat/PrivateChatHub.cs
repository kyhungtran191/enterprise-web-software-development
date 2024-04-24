using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Server.Application.Common.Dtos.PrivateChat;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Common.Interfaces.Services;
using Server.Domain.Entity.Identity;

namespace Server.Application.Common.Interfaces.Hubs.PrivateChat;

[Authorize]
public class PrivateChatHub : Hub
{
    private readonly ICurrentUserService _currentUserService;
    private readonly UserManager<AppUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public PrivateChatHub(ICurrentUserService currentUserService, IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
    {
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public override Task OnConnectedAsync()
    {
        var currentUserId = _currentUserService.UserId;

        if (!string.IsNullOrEmpty(currentUserId))
        {
            var currentUserFromDb = _userManager.FindByIdAsync(currentUserId).GetAwaiter().GetResult();

            if (currentUserFromDb != null)
            {
                Clients
                .Users(HubConnections.GetOnlineUsers())
                .SendAsync("NewUserConnected", new NewUserConnectedDto
                {
                    UserId = currentUserId,
                    Username = currentUserFromDb.UserName,
                });

                if (!HubConnections.HasUser(currentUserId))
                {
                    currentUserFromDb.IsOnline = true;
                    _userManager.UpdateAsync(currentUserFromDb).GetAwaiter().GetResult();
                }

                HubConnections.AddUserConnection(currentUserId, Context.ConnectionId);
            }
        }

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var currentUserId = _currentUserService.UserId;

        AppUser? currentUserFromDb = null;

        var hasCurrentUserId = !string.IsNullOrEmpty(currentUserId);

        if (hasCurrentUserId)
        {
            currentUserFromDb = _userManager.FindByIdAsync(currentUserId).GetAwaiter().GetResult();
        }

        if (HubConnections.HasUserConnection(currentUserId, Context.ConnectionId))
        {
            var userConnections = HubConnections.Users[currentUserId];
            userConnections.Remove(Context.ConnectionId);

            // delete if it just one connections has been deleted -> need clean up userId
            HubConnections.Users.Remove(currentUserId);

            if (userConnections.Any())
            {
                HubConnections.Users.Add(currentUserId, userConnections);
            }
            else
            {
                if (currentUserFromDb != null)
                {
                    currentUserFromDb!.IsOnline = false;
                    _userManager.UpdateAsync(currentUserFromDb).GetAwaiter().GetResult();
                }
            }
        }

        if (currentUserFromDb != null)
        {
            Clients
            .Users(HubConnections.GetOnlineUsers())
            .SendAsync("NewUserDisonnected", new NewUserDisconnectedDto
            {
                UserId = currentUserId,
                Username = currentUserFromDb.UserName,
            });
        }

        return base.OnDisconnectedAsync(exception);
    }

    public async Task Ping() {
        await Clients.Caller.SendAsync("Ping", "Ok");
    }
}