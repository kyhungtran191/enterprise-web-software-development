using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Application.Common.Dtos.PrivateChat;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Common.Interfaces.Services;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Content;
using Server.Domain.Entity.Identity;

namespace Server.Api.Controllers;

[Authorize]
[Route("[controller]")]
public class PrivateChatController : ApiController
{
    private readonly ICurrentUserService _currentUserService;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly UserManager<AppUser> _userManager;

    public PrivateChatController(ISender mediatorSender, ICurrentUserService currentUserService, UserManager<AppUser> userManager, IUnitOfWork unitOfWork, RoleManager<AppRole> roleManager, IDateTimeProvider dateTimeProvider) : base(mediatorSender)
    {

        _currentUserService = currentUserService;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _roleManager = roleManager;
        _dateTimeProvider = dateTimeProvider;
    }

    [HttpGet]
    [Route("faculty-users")]
    public async Task<IActionResult> GetAllFaculyUsers()
    {
        var currentUserId = _currentUserService.UserId;
        var currentUser = await _userManager.FindByIdAsync(currentUserId);

        if (currentUser == null)
        {
            return Problem(new List<ErrorOr.Error> {
                Errors.User.CannotFound
            });
        }

        var faculty = _unitOfWork.FacultyRepository.Find(x => x.Id == currentUser!.FacultyId).FirstOrDefault();

        if (faculty == null)
        {
            return Problem(new List<ErrorOr.Error> {
                Errors.Faculty.CannotFound
            });
        }

        var usersWithFaculty =
            from x in await _userManager.Users.ToListAsync()
            join y in await _unitOfWork.FacultyRepository.GetAllAsync()
            on x.FacultyId equals y.Id
            where y.Name == faculty!.Name
            select x;

        List<PrivateChatUserDto> result = new();

        foreach (var receiver in usersWithFaculty)
        {
            var roles = await _userManager.GetRolesAsync(receiver);

            result.Add(new PrivateChatUserDto
            {
                CurrentUserId = currentUserId,
                Avatar = receiver.Avatar,
                IsOnline = receiver.IsOnline,
                ReceiverId = receiver.Id,
                Username = receiver.UserName,
                Role = roles[0]
            });
        }

        return Ok(result);
    }

    [HttpPost]
    [Route("conservation-users")]
    public async Task<IActionResult> CreateConversation([FromQuery] string specificReceiverId)
    {
        var specificReceiver = await _userManager.FindByIdAsync(specificReceiverId);

        if (specificReceiver == null)
        {
            return Problem(new List<ErrorOr.Error> {
                Errors.User.CannotFound
            });
        }

        var currentUserId = _currentUserService.UserId;
        // var hasConversation = await _unitOfWork.PrivateChatRepository.HasConversation(currentUserId, specificReceiverId);
        var conversation = 
            _unitOfWork
            .PrivateChatRepository
            .Find(privateChat => 
                (privateChat.User1Id.ToString() == currentUserId && privateChat.User2Id.ToString() == specificReceiverId) || 
                (privateChat.User1Id.ToString() == specificReceiverId && privateChat.User2Id.ToString() == currentUserId))
            .FirstOrDefault();

        if (conversation == null)
        {
            try
            {
                var currentUserIdGuid = Guid.Parse(currentUserId);
                var specificReceiverIdGuid = Guid.Parse(specificReceiverId);

                _unitOfWork.PrivateChatRepository.Add(new PrivateChat
                {
                    User1Id = currentUserIdGuid,
                    User2Id = specificReceiverIdGuid,
                    LastActivity = _dateTimeProvider.UtcNow,
                });

                await _unitOfWork.CompleteAsync();

                return Ok(true);
            }
            catch
            {
                return Problem(new List<ErrorOr.Error> { Errors.Parse.CannotParse });
            }
        }
        else
        {
            conversation.LastActivity = _dateTimeProvider.UtcNow;

            await _unitOfWork.CompleteAsync();
        }

        return Ok(false);
    }

    // render users and specific user with messages
    [HttpGet]
    [Route("conservation-users")]
    public async Task<IActionResult> GetAllConversationUsers([FromQuery] string? specificReceiverId)
    {
        var currentUserId = _currentUserService.UserId;
        var currentUser = await _userManager.FindByIdAsync(currentUserId);

        var privateChats = await _unitOfWork.PrivateChatRepository.GetAllUsers(currentUserId);

        var receiverIdMapChatId = new Dictionary<string, string>();

        foreach (var privateChat in privateChats)
        {
            if (privateChat.User1Id.ToString() == currentUserId)
            {
                receiverIdMapChatId.Add(privateChat.User2Id.ToString(), privateChat.Id.ToString());
            }
            else
            {
                receiverIdMapChatId.Add(privateChat.User1Id.ToString(), privateChat.Id.ToString());
            }
        }

        // var userId2s = privateChats.Select(x => x.User2Id.ToString()).ToList();
        // var userId1s = privateChats.Select(x => x.User1Id.ToString()).ToList();

        // var receiverIds =
        //     userId1s
        //     .Union(userId2s)
        //     .Where(x => x != currentUserId)
        //     .ToList();

        var receiverIds = receiverIdMapChatId.Keys.ToList();

        bool hasSpecificReceiverId = !string.IsNullOrEmpty(specificReceiverId);

        if (hasSpecificReceiverId)
        {
            if (!receiverIds.Contains(specificReceiverId!))
            {
                return Problem(new List<ErrorOr.Error>
                {
                    Errors.User.CannotFound
                });
            }
        }

        // get all receivers
        List<AppUser> receivers = new();

        foreach (var receiverId in receiverIds)
        {
            var receiver = await _userManager.FindByIdAsync(receiverId);

            if (receiver != null)
            {
                receivers.Add(receiver);
            }
        }

        // mAp to dto and messages to first receiver or specific user
        List<PrivateChatUserDto> result = new();

        for (int i = 0; i < receivers.Count; ++i)
        {
            var receiver = receivers[i];

            var roles = await _userManager.GetRolesAsync(receiver);

            var privateChatUserDto = new PrivateChatUserDto
            {
                CurrentUserId = currentUserId,
                Avatar = receiver.Avatar,
                IsOnline = receiver.IsOnline,
                ReceiverId = receiver.Id,
                Username = receiver.UserName,
                Role = roles[0],
                ChatId = receiverIdMapChatId[receiver.Id.ToString()]
            };

            if (hasSpecificReceiverId && receiver.Id.ToString() == specificReceiverId)
            {
                privateChatUserDto.CurrentMessagesReceiver = GetPrivateMessageDtos(currentUserId,
                                                                                   receiver.Id.ToString(),
                                                                                   receiverIdMapChatId[receiver.Id.ToString()],
                                                                                   currentUser?.Avatar!,
                                                                                   receiver.Avatar!);
            }

            result.Add(privateChatUserDto);
        }

        if (receivers.Count > 0 && !hasSpecificReceiverId)
        {
            var firstReceiver = receivers[0];
            result[0].CurrentMessagesReceiver = GetPrivateMessageDtos(currentUserId,
                                                                      firstReceiver.Id.ToString(),
                                                                      receiverIdMapChatId[firstReceiver.Id.ToString()],
                                                                      currentUser?.Avatar!,
                                                                      firstReceiver.Avatar!);
        }

        return Ok(result);
    }

    private List<PrivateMessageDto> GetPrivateMessageDtos(string userId,
                                                        string receiverId,
                                                        string chatId,
                                                        string avatarSender,
                                                        string avatarReceiver)
    {
        var messages = _unitOfWork
            .PrivateMessagesRepository
            .Find(x =>
                (x.SenderId.ToString() == userId && x.ReceiverId.ToString() == receiverId) ||
                x.SenderId.ToString() == receiverId && x.ReceiverId.ToString() == userId)
            .OrderByDescending(x => x.DateCreated);

        return messages.Select(x => new PrivateMessageDto
        {
            SenderId = x.SenderId,
            ReceiverId = x.ReceiverId,
            AvatarReceiver = avatarReceiver,
            AvatarSender = avatarSender,
            DateCreated = x.DateCreated,
            Content = x.Content,
            ChatId = chatId,
        }).ToList();
    }

    [HttpPost]
    [Route("message")]
    public async Task<IActionResult> SendMessage([FromBody] SendPrivateMessageDto privateMessageDto)
    {
        // var conversation = await _unitOfWork.PrivateChatRepository.HasConversation(privateMessageDto.SenderId!, privateMessageDto.ReceiverId!);
        var conversation = _unitOfWork.PrivateChatRepository.Find(x => x.Id.ToString() == privateMessageDto.ChatId).FirstOrDefault();

        if (conversation == null)
        {
            return Problem(new List<ErrorOr.Error> {
                Errors.Chat.CannotFound
            });
        }


        var sender = await _userManager.FindByIdAsync(privateMessageDto.SenderId!);

        if (sender == null)
        {
            return Problem(new List<ErrorOr.Error> {
                Errors.Chat.SenderCannotFound
            });
        }

        var receiver = await _userManager.FindByIdAsync(privateMessageDto.ReceiverId!);

        if (receiver == null)
        {
            return Problem(new List<ErrorOr.Error> {
                Errors.Chat.ReceiverCannotFound
            });
        }

        conversation.LastActivity = _dateTimeProvider.UtcNow;

        _unitOfWork.PrivateMessagesRepository.Add(new PrivateMessage
        {
            ChatId = Guid.Parse(privateMessageDto.ChatId!),
            SenderId = sender.Id,
            ReceiverId = receiver.Id,
            Content = privateMessageDto.Content,
        });

        await _unitOfWork.CompleteAsync();

        // web socket

        return Ok();
    }

}