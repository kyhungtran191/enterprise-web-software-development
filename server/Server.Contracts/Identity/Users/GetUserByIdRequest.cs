using Microsoft.AspNetCore.Mvc;

namespace Server.Contracts.Identity.Users;

public class GetUserByIdRequest
{
    [FromRoute]
    public Guid Id { get; set; }
}