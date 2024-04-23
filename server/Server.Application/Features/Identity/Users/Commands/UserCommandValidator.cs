using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace Server.Application.Features.Identity.Users.Commands;

//    public string? FirstName { get; set; }
//     public string? LastName { get; set; }
//     public string? PhoneNumber { get; set; }
//     public DateTime? Dob { get; set; }
//     public string? Avatar { get; set; }
//     public bool IsActive { get; set; }
public class UserCommandValidator<T> 
    : AbstractValidator<T> where T : UserCommandBase
{
    public UserCommandValidator()
    {
        //RuleFor(x => x.FirstName)
        //    .MaximumLength(255)
        //        .WithMessage("First name must be less than 255 characters")
        //    .NotEmpty()
        //        .WithMessage("First name must not be empty.");

        //RuleFor(x => x.LastName)
        //    .MaximumLength(255)
        //        .WithMessage("First name must be less than 255 characters")
        //    .NotEmpty()
        //        .WithMessage("First name must not be empty.");

        //RuleFor(x => x.PhoneNumber)
        //    .MaximumLength(11)
        //        .WithMessage("Phone Number have to 11 characters")
        //    .NotEmpty()
        //        .WithMessage("Phone Number must not be empty.");
    }
}