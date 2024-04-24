using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Features.Identity.Users.Commands.CreateGuest
{
    public class CreateGuestCommand : UserCommandBase
    {
        public string Email { get; set; } = default!;
        public string UserName { get; set; } = default!;
    }
}
