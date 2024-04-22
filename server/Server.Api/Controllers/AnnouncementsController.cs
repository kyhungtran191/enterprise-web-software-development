using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Common.Extensions;
using Server.Application.Common.Interfaces.Services;

namespace Server.Api.Controllers;

[Authorize]
[Route("[controller]")]
public class AnnouncementsController : ApiController
{
    private readonly IAnnouncementService _announcementService;

    public AnnouncementsController(ISender mediatorSender, 
                                  IAnnouncementService announcementService) : base(mediatorSender)
    {
        _announcementService = announcementService;
    }


    [HttpGet]
    [Route("paging")]
    public async Task<IActionResult> GetAllPaging(int pageIndex, int pageSize)
    {
        var model = await _announcementService.GetAllUnreadPaging(User.GetUserId(), pageIndex, pageSize);
        return Ok(model);
    }

    [HttpPost]
    public async Task<IActionResult> MarkAsRead(string id)
    {
        var result = await _announcementService.MarkAsRead(User.GetUserId(), id);

        return Ok(result);
    }
}