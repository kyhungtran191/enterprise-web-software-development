using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contracts.Identity.Users
{
    public class CreateGuestRequest
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        //public string PhoneNumber { get; set; } = default!;
       
        //public Guid FacultyId { get; set; }
        //public Guid RoleId { get; set; } = default!;
        //public DateTime? Dob { get; set; }
        //public IFormFile? Avatar { get; set; }
        public bool IsActive { get; set; }
    }
}
