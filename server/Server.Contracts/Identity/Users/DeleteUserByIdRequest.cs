using Microsoft.AspNetCore.Mvc;

namespace Server.Contracts.Identity.Users;

public class DeleteUserByIdRequest
{
    [FromRoute]
    public Guid Id { get; set; }
}