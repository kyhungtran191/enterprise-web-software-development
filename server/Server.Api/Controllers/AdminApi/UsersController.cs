using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Server.Api.Controllers.AdminApi;

public class UsersController : AdminApiController
{
    private readonly IMapper _mapper;
    public UsersController(ISender mediatorSender, IMapper mapper) : base(mediatorSender)
    {
        _mapper = mapper;
    }

    [HttpGet]    
    public IActionResult GetAllUsers() {
        return Ok("All Users");
    }

}