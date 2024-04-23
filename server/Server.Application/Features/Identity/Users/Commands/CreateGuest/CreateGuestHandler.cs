using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Server.Application.Common.Extensions;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Common.Interfaces.Services;
using Server.Application.Wrappers;
using Server.Contracts.Common;
using Server.Domain.Common.Constants;
using Server.Domain.Common.Errors;
using Server.Domain.Entity.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Server.Application.Features.Identity.Users.Commands.CreateGuest
{
    public class CreateGuestHandler : IRequestHandler<CreateGuestCommand, ErrorOr<IResponseWrapper>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediaService _mediaService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        public CreateGuestHandler(UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            IMapper mapper,
            IEmailService emailService,
            IUnitOfWork unitOfWork, IMediaService mediaService, IConfiguration configuration)
        {
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _mediaService = mediaService;
            _emailService = emailService;
            _configuration = configuration;
        }
        public async Task<ErrorOr<IResponseWrapper>> Handle(CreateGuestCommand request, CancellationToken cancellationToken)
        {
            if (await _userManager.FindByEmailAsync(request.Email) is not null)
            {
                return Errors.User.DuplicateEmail;
            }

            var role = await _roleManager.FindByNameAsync(Roles.Guest);

            if (role is null)
            {
                return Errors.Roles.NotFound;
            }
            var facultyFromDb =
                await _unitOfWork
                .FacultyRepository
                .GetByIdAsync(request.FacultyId);

            if (facultyFromDb is null)
            {
                return Errors.Faculty.CannotFound;
            }
            var newUser = new AppUser();

            _mapper.Map(request, newUser);
            newUser.Id = Guid.NewGuid();
            newUser.FacultyId = facultyFromDb.Id;
            string randomPassword = RandomString(8);
            newUser.PasswordHash = new PasswordHasher<AppUser>().HashPassword(newUser, randomPassword);

            // avatar
            if (request.Avatar is not null)
            {
                var avatarList = new List<IFormFile>();
                avatarList.Add(request.Avatar);
                var avatarInfo = await _mediaService.UploadFileCloudinary(avatarList, FileType.Avatar, newUser.Id);
                foreach (var info in avatarInfo)
                {
                    newUser.Avatar = info.Path;
                    newUser.AvatarPublicId = info.PublicId;
                }
            }

            var result = await _userManager.CreateAsync(newUser);

            if (!result.Succeeded)
            {
                return result.GetIdentityResultErrorDescriptions();
            }

            result = await _userManager.AddToRoleAsync(newUser, role.Name!);

            if (!result.Succeeded)
            {
                return result.GetIdentityResultErrorDescriptions();
            }

            // send mail
            var token = await _userManager.GeneratePasswordResetTokenAsync(newUser);
            var resetPasswordBaseUrl = _configuration["ApplicationSettings:ResetPasswordBaseUrl"];
            var resetPasswordUrl = $"{resetPasswordBaseUrl}/{Uri.EscapeDataString(token)}";
            _emailService.SendEmail(new MailRequest
            {
                ToEmail = request.Email,
                Subject = "University Provide Password",
                Body = $"Email: <h1>{request.Email}</h1>, Password: <h1>{randomPassword}</h1>.<br> If you want to reset your password, click the link below: <br> {resetPasswordUrl} "
            });

            return new ResponseWrapper
            {
                IsSuccessfull = true,
                Messages = new List<string>
                {
                    "Create new user successfully!"
                }
            };
        }
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
