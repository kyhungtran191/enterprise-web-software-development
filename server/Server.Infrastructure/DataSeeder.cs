﻿using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Server.Application.Common.Dtos;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Common.Extensions;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Common.Interfaces.Services;
using Server.Domain.Common.Constants;
using Server.Domain.Entity.Content;
using Server.Domain.Entity.Identity;
using Server.Infrastructure.Persistence.Repositories;
using Server.Infrastructure.Services;
using File = Server.Domain.Entity.Content.File;

namespace Server.Infrastructure;

public static class DataSeeder
{

    public static async Task SeedAsync(AppDbContext context,
                                       RoleManager<AppRole> roleManager)
    {
        var passwordHasher = new PasswordHasher<AppUser>();

        var rootAdminRoleId = Guid.NewGuid();

        AppRole? role = new AppRole
        {
            Id = rootAdminRoleId,
            Name = Roles.Admin,
            NormalizedName = Roles.Admin.ToUpperInvariant(),
            DisplayName = "Admisnistrator",
        }; ;

        if (!context.Roles.Any())
        {
            await context.Roles.AddAsync(role);
            await context.SaveChangesAsync();
        }

        if (!context.Users.Any())
        {
            var userId = Guid.NewGuid();
            var userEmail = "admin@gmail.com";
            var userName = "admin";
            var user = new AppUser
            {
                Id = userId,
                FirstName = "An",
                LastName = "Minh",
                Email = userEmail,
                NormalizedEmail = userEmail.ToUpperInvariant(),
                UserName = userName,
                NormalizedUserName = userName.ToUpperInvariant(),
                IsActive = true,
                // ko co cai nay ko login duoc
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                DateCreated = DateTime.Now,
            };
            user.PasswordHash = passwordHasher.HashPassword(user, "Admin123@");

            await context.Users.AddAsync(user);

            await context.UserRoles.AddAsync(new IdentityUserRole<Guid>
            {
                RoleId = rootAdminRoleId,
                UserId = userId
            });

            await context.SaveChangesAsync();
        }

        // seed permissions        
        if (context.RoleClaims.Any() == false)
        {
            var permissions = await roleManager.GetClaimsAsync(role);

            if (permissions.Any() == false)
            {
                var allPermisisons = new List<RoleClaimsDto>();

                var types =
                    typeof(Permissions)
                    .GetTypeInfo()
                    .DeclaredNestedTypes
                    .ToList();

                types.ForEach(allPermisisons.GetPermissionsByType);

                foreach (var permission in allPermisisons)
                {
                    await roleManager.AddClaimAsync(role, new Claim("permissions", permission.Value!));
                }
            }
        }
    }

    public static async Task SeedContribution(AppDbContext context, 
    RoleManager<AppRole> roleManager, 
    IContributionRepository contributionRepository, 
    IRatingRepository ratingRepository,
    IDateTimeProvider dateTimeProvider,
    IPrivateChatRepository privateChatRepository, 
    IPrivateMessagesRepository privateMessagesRepository)
    {
        var allFaculties = new List<string>
        {
            Faculties.Bussiness,
            Faculties.IT,
            Faculties.Design,
            Faculties.Marketing,
        };
        var facultiesList = new List<Faculty>
        {
            new() { Id = Guid.NewGuid(), Name = Faculties.Bussiness, Icon = "DefaultIcon" },
            new() { Id = Guid.NewGuid(), Name = Faculties.IT, Icon = "DefaultIcon" },
            new() { Id = Guid.NewGuid(), Name = Faculties.Design, Icon = "DefaultIcon" },
            new() { Id = Guid.NewGuid(), Name = Faculties.Marketing, Icon = "DefaultIcon" },
            new() { Id = Guid.NewGuid(), Name = Faculties.Art, Icon = "DefaultIcon" },
        };
        if (!context.Faculties.Any())
        {
            foreach (var faculty in facultiesList)
            {
                await context.Faculties.AddAsync(faculty);
            }

            await context.SaveChangesAsync();
        }
        var yearList = new List<AcademicYear>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "2021-2022", StartClosureDate = new DateTime(2021, 1, 1),
                EndClosureDate = new DateTime(2021, 7, 1), FinalClosureDate = new DateTime(2022, 10, 1),
                UserNameCreated = "Default",DateCreated = DateTime.Now
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "2022-2023", StartClosureDate = new DateTime(2022, 1, 1),
                EndClosureDate = new DateTime(2022, 7, 1), FinalClosureDate = new DateTime(2022, 10, 1),
                UserNameCreated = "Default",DateCreated = DateTime.Now
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "2023-2024", StartClosureDate = new DateTime(2023, 1, 1),
                EndClosureDate = new DateTime(2023, 7, 1), FinalClosureDate = new DateTime(2023, 10, 1),
                UserNameCreated = "Default",DateCreated = DateTime.Now
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "2024-2025", StartClosureDate = new DateTime(2024, 1, 1),
                EndClosureDate = new DateTime(2024, 7, 1), FinalClosureDate = new DateTime(2024, 10, 1),
                UserNameCreated = "Default",DateCreated = DateTime.Now,
                IsActive = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "2025-2026", StartClosureDate = new DateTime(2025, 1, 1),
                EndClosureDate = new DateTime(2025, 7, 1), FinalClosureDate = new DateTime(2025, 10, 1),
                UserNameCreated = "Default",DateCreated = DateTime.Now
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "2026-2027", StartClosureDate = new DateTime(2026, 1, 1),
                EndClosureDate = new DateTime(2026, 7, 1), FinalClosureDate = new DateTime(2026, 10, 1),
                UserNameCreated = "Default",DateCreated = DateTime.Now
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "2027-2028", StartClosureDate = new DateTime(2027, 1, 1),
                EndClosureDate = new DateTime(2027, 7, 1), FinalClosureDate = new DateTime(2027, 10, 1),
                UserNameCreated = "Default",DateCreated = DateTime.Now
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "2028-2029", StartClosureDate = new DateTime(2028, 1, 1),
                EndClosureDate = new DateTime(2028, 7, 1), FinalClosureDate = new DateTime(2028, 10, 1),
                UserNameCreated = "Default",DateCreated = DateTime.Now
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "2029-2030", StartClosureDate = new DateTime(2029, 1, 1),
                EndClosureDate = new DateTime(2029, 7, 1), FinalClosureDate = new DateTime(2029, 10, 1),
                UserNameCreated = "Default",DateCreated = DateTime.Now
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "2030-2031", StartClosureDate = new DateTime(2030, 1, 1),
                EndClosureDate = new DateTime(2030, 7, 1), FinalClosureDate = new DateTime(2030, 10, 1),
                UserNameCreated = "Default",DateCreated = DateTime.Now
            },

        };


        if (!context.AcademicYears.Any())
        {
            foreach (var year in yearList)
            {
                await context.AcademicYears.AddAsync(year);
            }

            await context.SaveChangesAsync();
        }

        // seed user
        var passwordHasher = new PasswordHasher<AppUser>();

        var rootAdminRoleId = Guid.NewGuid();
        var adminId = Guid.NewGuid();
        var managerId = Guid.NewGuid();
        var studentRoleId = Guid.NewGuid();
        var coordinatorRoleId = Guid.NewGuid();
        var guestRoleId = Guid.NewGuid();
        var managerRoleId = Guid.NewGuid();
        AppRole? role = new AppRole
        {
            Id = rootAdminRoleId,
            Name = Roles.Admin,
            NormalizedName = Roles.Admin.ToUpperInvariant(),
            DisplayName = "Administrator",
        };
        AppRole? studentRole = new AppRole
        {
            Id = studentRoleId,
            Name = Roles.Student,
            NormalizedName = Roles.Student.ToUpperInvariant(),
            DisplayName = "Student",
        };
        AppRole? coordinatorRole = new AppRole
        {
            Id = coordinatorRoleId,
            Name = Roles.Coordinator,
            NormalizedName = Roles.Coordinator.ToUpperInvariant(),
            DisplayName = "Marketing Coordinator",
        };
        AppRole? guestRole = new AppRole
        {
            Id = guestRoleId,
            Name = Roles.Guest,
            NormalizedName = Roles.Guest.ToUpperInvariant(),
            DisplayName = "Guest",
        };
        AppRole? managerRole = new AppRole
        {
            Id = managerRoleId,
            Name = Roles.Manager,
            NormalizedName = Roles.Manager.ToUpperInvariant(),
            DisplayName = "Manager"
        };

        if (!context.Roles.Any())
        {
            await context.Roles.AddAsync(role);
            await context.Roles.AddAsync(studentRole);
            await context.Roles.AddAsync(coordinatorRole);
            await context.Roles.AddAsync(guestRole);
            await context.Roles.AddAsync(managerRole);
            await context.SaveChangesAsync();
        }

        var student1_gmail_com = Guid.NewGuid();

        var studentList = new List<AppUser>
        {
            new()
            {
                Id = Guid.NewGuid(), FirstName = "An", LastName = "Minh", Email = "student1@gmail.com",
                NormalizedEmail = "student1@gmail.com".ToUpperInvariant(), UserName = "student1",
                NormalizedUserName = "student1".ToUpperInvariant(), IsActive = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                DateCreated = DateTime.Now,
                FacultyId = facultiesList[0].Id,
                Avatar = "http://res.cloudinary.com/dlqxj0ibb/image/upload/v1712572469/avatar/user-7f65f0e5-fc13-4cea-bff6-677844b5871a/fontiyvnmsbxq5wjdbqu.jpg",
                AvatarPublicId = "avatar/user-7f65f0e5-fc13-4cea-bff6-677844b5871a/fontiyvnmsbxq5wjdbqu"

            },
            new()
            {
                Id = student1_gmail_com, FirstName = "Vu", LastName = "Nguyen", Email = "trankyhung225@gmail.com",
                NormalizedEmail = "trankyhung225@gmail.com".ToUpperInvariant(), UserName = "trankyhung225",
                NormalizedUserName = "trankyhung225".ToUpperInvariant(), IsActive = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                DateCreated = DateTime.Now,
                FacultyId = facultiesList[1].Id,
                Avatar = "http://res.cloudinary.com/dlqxj0ibb/image/upload/v1712572469/avatar/user-7f65f0e5-fc13-4cea-bff6-677844b5871a/fontiyvnmsbxq5wjdbqu.jpg",
                AvatarPublicId = "avatar/user-7f65f0e5-fc13-4cea-bff6-677844b5871a/fontiyvnmsbxq5wjdbqu"

            },
            new()
            {
                Id = Guid.NewGuid(), FirstName = "Hung", LastName = "Tran", Email = "student2@gmail.com",
                NormalizedEmail = "student2@gmail.com".ToUpperInvariant(), UserName = "student2",
                NormalizedUserName = "student2".ToUpperInvariant(), IsActive = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                DateCreated = DateTime.Now,
                FacultyId = facultiesList[2].Id,
                Avatar = "http://res.cloudinary.com/dlqxj0ibb/image/upload/v1712572469/avatar/user-7f65f0e5-fc13-4cea-bff6-677844b5871a/fontiyvnmsbxq5wjdbqu.jpg",
                AvatarPublicId = "avatar/user-7f65f0e5-fc13-4cea-bff6-677844b5871a/fontiyvnmsbxq5wjdbqu"

            },
            new()
            {
                Id = Guid.NewGuid(), FirstName = "Khang", LastName = "Nguyen", Email = "student3@gmail.com",
                NormalizedEmail = "student3@gmail.com".ToUpperInvariant(), UserName = "student3",
                NormalizedUserName = "student3".ToUpperInvariant(), IsActive = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                DateCreated = DateTime.Now,
                FacultyId = facultiesList[3].Id,
                Avatar = "http://res.cloudinary.com/dlqxj0ibb/image/upload/v1712572469/avatar/user-7f65f0e5-fc13-4cea-bff6-677844b5871a/fontiyvnmsbxq5wjdbqu.jpg",
                AvatarPublicId = "avatar/user-7f65f0e5-fc13-4cea-bff6-677844b5871a/fontiyvnmsbxq5wjdbqu"

            },
             new()
            {
                Id = Guid.NewGuid(), FirstName = "An 2", LastName = "Minh 2", Email = "student4@gmail.com",
                NormalizedEmail = "student4@gmail.com".ToUpperInvariant(), UserName = "student4",
                NormalizedUserName = "student4".ToUpperInvariant(), IsActive = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                DateCreated = DateTime.Now,
                FacultyId = facultiesList[0].Id,
                Avatar = "http://res.cloudinary.com/dlqxj0ibb/image/upload/v1712572469/avatar/user-7f65f0e5-fc13-4cea-bff6-677844b5871a/fontiyvnmsbxq5wjdbqu.jpg",
                AvatarPublicId = "avatar/user-7f65f0e5-fc13-4cea-bff6-677844b5871a/fontiyvnmsbxq5wjdbqu"

            },
            new()
            {
                Id = Guid.NewGuid(), FirstName = "Vu 2", LastName = "Nguyen 2", Email = "student5@gmail.com",
                NormalizedEmail = "student5@gmail.com".ToUpperInvariant(), UserName = "student5",
                NormalizedUserName = "student5".ToUpperInvariant(), IsActive = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                DateCreated = DateTime.Now,
                FacultyId = facultiesList[1].Id,
                Avatar = "http://res.cloudinary.com/dlqxj0ibb/image/upload/v1712572469/avatar/user-7f65f0e5-fc13-4cea-bff6-677844b5871a/fontiyvnmsbxq5wjdbqu.jpg",
                AvatarPublicId = "avatar/user-7f65f0e5-fc13-4cea-bff6-677844b5871a/fontiyvnmsbxq5wjdbqu"

            },
            new()
            {
                Id = Guid.NewGuid(), FirstName = "Hung2", LastName = "Tran 2", Email = "student6@gmail.com",
                NormalizedEmail = "student6@gmail.com".ToUpperInvariant(), UserName = "student6",
                NormalizedUserName = "student6".ToUpperInvariant(), IsActive = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                DateCreated = DateTime.Now,
                FacultyId = facultiesList[2].Id,
                Avatar = "http://res.cloudinary.com/dlqxj0ibb/image/upload/v1712572469/avatar/user-7f65f0e5-fc13-4cea-bff6-677844b5871a/fontiyvnmsbxq5wjdbqu.jpg",
                AvatarPublicId = "avatar/user-7f65f0e5-fc13-4cea-bff6-677844b5871a/fontiyvnmsbxq5wjdbqu"

            },
            new()
            {
                Id = Guid.NewGuid(), FirstName = "Khang 2", LastName = "Nguyen 2", Email = "student7@gmail.com",
                NormalizedEmail = "student7@gmail.com".ToUpperInvariant(), UserName = "student7",
                NormalizedUserName = "student3".ToUpperInvariant(), IsActive = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                DateCreated = DateTime.Now,
                FacultyId = facultiesList[3].Id,
                Avatar = "http://res.cloudinary.com/dlqxj0ibb/image/upload/v1712572469/avatar/user-7f65f0e5-fc13-4cea-bff6-677844b5871a/fontiyvnmsbxq5wjdbqu.jpg",
                AvatarPublicId = "avatar/user-7f65f0e5-fc13-4cea-bff6-677844b5871a/fontiyvnmsbxq5wjdbqu"

            },
            new()
            {
                Id = Guid.NewGuid(), FirstName = "Hung 3", LastName = "Tran3", Email = "student8@gmail.com",
                NormalizedEmail = "student8@gmail.com".ToUpperInvariant(), UserName = "student8",
                NormalizedUserName = "student8".ToUpperInvariant(), IsActive = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                DateCreated = DateTime.Now,
                FacultyId = facultiesList[0].Id,
                Avatar = "http://res.cloudinary.com/dlqxj0ibb/image/upload/v1712572469/avatar/user-7f65f0e5-fc13-4cea-bff6-677844b5871a/fontiyvnmsbxq5wjdbqu.jpg",
                AvatarPublicId = "avatar/user-7f65f0e5-fc13-4cea-bff6-677844b5871a/fontiyvnmsbxq5wjdbqu"

            },
            new()
            {
                Id = Guid.NewGuid(), FirstName = "Khang 3", LastName = "Nguyen 3", Email = "student9@gmail.com",
                NormalizedEmail = "student9@gmail.com".ToUpperInvariant(), UserName = "student9",
                NormalizedUserName = "student9".ToUpperInvariant(), IsActive = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                DateCreated = DateTime.Now,
                FacultyId = facultiesList[1].Id,
                Avatar = "http://res.cloudinary.com/dlqxj0ibb/image/upload/v1712572469/avatar/user-7f65f0e5-fc13-4cea-bff6-677844b5871a/fontiyvnmsbxq5wjdbqu.jpg",
                AvatarPublicId = "avatar/user-7f65f0e5-fc13-4cea-bff6-677844b5871a/fontiyvnmsbxq5wjdbqu"

            },
            new()
            {
                Id = Guid.NewGuid(), FirstName = "Minh", LastName = "Nguyen", Email = "student10@gmail.com",
                NormalizedEmail = "student10@gmail.com".ToUpperInvariant(), UserName = "student10",
                NormalizedUserName = "student10".ToUpperInvariant(), IsActive = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                DateCreated = DateTime.Now,
                FacultyId = facultiesList[4].Id,
                Avatar = "http://res.cloudinary.com/dlqxj0ibb/image/upload/v1712572469/avatar/user-7f65f0e5-fc13-4cea-bff6-677844b5871a/fontiyvnmsbxq5wjdbqu.jpg",
                AvatarPublicId = "avatar/user-7f65f0e5-fc13-4cea-bff6-677844b5871a/fontiyvnmsbxq5wjdbqu"

            },
        };

        var cor_hungtkgcs200234_fpt_edu_vn = Guid.NewGuid();


        var coordinatorList = new List<AppUser>
        {
            new()
            {
                Id = Guid.NewGuid(), FirstName = "An", LastName = "Minh", Email = "coordinator@gmail.com",
                NormalizedEmail = "coordinator@gmail.com".ToUpperInvariant(), UserName = "coordinator",
                NormalizedUserName = "coordinator".ToUpperInvariant(), IsActive = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                DateCreated = DateTime.Now,
                FacultyId = facultiesList[0].Id,
                Avatar = "http://res.cloudinary.com/dlqxj0ibb/image/upload/v1712572623/avatar/user-53555e63-6adc-4935-87a2-005c73af2a45/pxeslpvopryeg6v0prn8.jpg",
                AvatarPublicId = "avatar/user-53555e63-6adc-4935-87a2-005c73af2a45/pxeslpvopryeg6v0prn8"

            },
            new()
            {
                Id = cor_hungtkgcs200234_fpt_edu_vn, FirstName = "Vu", LastName = "Nguyen", Email = "hungtkgcs200234@fpt.edu.vn",
                NormalizedEmail = "hungtkgcs200234@fpt.edu.vn".ToUpperInvariant(), UserName = "coordinator1",
                NormalizedUserName = "coordinator1".ToUpperInvariant(), IsActive = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                DateCreated = DateTime.Now,
                FacultyId = facultiesList[1].Id,
                Avatar = "http://res.cloudinary.com/dlqxj0ibb/image/upload/v1712572623/avatar/user-53555e63-6adc-4935-87a2-005c73af2a45/pxeslpvopryeg6v0prn8.jpg",
                AvatarPublicId = "avatar/user-53555e63-6adc-4935-87a2-005c73af2a45/pxeslpvopryeg6v0prn8"

            },
            new()
            {
                Id = Guid.NewGuid(), FirstName = "Vu", LastName = "Nguyen2", Email = "vunguyenNguaHoang2k3@fpt.edu.vn",
                NormalizedEmail = "vunguyenNguaHoang2k3@fpt.edu.vn".ToUpperInvariant(), UserName = "coordinator5",
                NormalizedUserName = "coordinator5".ToUpperInvariant(), IsActive = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                DateCreated = DateTime.Now,
                FacultyId = facultiesList[1].Id,
                Avatar = "http://res.cloudinary.com/dlqxj0ibb/image/upload/v1712572623/avatar/user-53555e63-6adc-4935-87a2-005c73af2a45/pxeslpvopryeg6v0prn8.jpg",
                AvatarPublicId = "avatar/user-53555e63-6adc-4935-87a2-005c73af2a45/pxeslpvopryeg6v0prn8"
            },
            new()
            {
                Id = Guid.NewGuid(), FirstName = "Hung", LastName = "Tran", Email = "nguyenvu260703.dev@gmail.com",
                NormalizedEmail = "nguyenvu260703.dev@gmail.com".ToUpperInvariant(), UserName = "coordinator2",
                NormalizedUserName = "coordinator2".ToUpperInvariant(), IsActive = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                DateCreated = DateTime.Now,
                FacultyId = facultiesList[2].Id,
                Avatar = "http://res.cloudinary.com/dlqxj0ibb/image/upload/v1712572623/avatar/user-53555e63-6adc-4935-87a2-005c73af2a45/pxeslpvopryeg6v0prn8.jpg",
                AvatarPublicId = "avatar/user-53555e63-6adc-4935-87a2-005c73af2a45/pxeslpvopryeg6v0prn8"

            },
            new()
            {
                Id = Guid.NewGuid(), FirstName = "Khang", LastName = "Nguyen", Email = "coordinator3@gmail.com",
                NormalizedEmail = "coordinator3@gmail.com".ToUpperInvariant(), UserName = "coordinator3",
                NormalizedUserName = "coordinator3".ToUpperInvariant(), IsActive = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                DateCreated = DateTime.Now,
                FacultyId = facultiesList[3].Id,
                Avatar = "http://res.cloudinary.com/dlqxj0ibb/image/upload/v1712572623/avatar/user-53555e63-6adc-4935-87a2-005c73af2a45/pxeslpvopryeg6v0prn8.jpg",
                AvatarPublicId = "avatar/user-53555e63-6adc-4935-87a2-005c73af2a45/pxeslpvopryeg6v0prn8"

            },
            new()
            {
                Id = Guid.NewGuid(), FirstName = "Khang 2", LastName = "Nguyen 2", Email = "coordinator4@gmail.com",
                NormalizedEmail = "coordinator4@gmail.com".ToUpperInvariant(), UserName = "coordinator4",
                NormalizedUserName = "coordinator4".ToUpperInvariant(), IsActive = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                DateCreated = DateTime.Now,
                FacultyId = facultiesList[4].Id,
                Avatar = "http://res.cloudinary.com/dlqxj0ibb/image/upload/v1712572623/avatar/user-53555e63-6adc-4935-87a2-005c73af2a45/pxeslpvopryeg6v0prn8.jpg",
                AvatarPublicId = "avatar/user-53555e63-6adc-4935-87a2-005c73af2a45/pxeslpvopryeg6v0prn8"

            },
        };
        var guestList = new List<AppUser>
        {
            new()
            {
                Id = Guid.NewGuid(), FirstName = "An", LastName = "Minh", Email = "guest@gmail.com",
                NormalizedEmail = "guest@gmail.com".ToUpperInvariant(), UserName = "guest",
                NormalizedUserName = "guest".ToUpperInvariant(), IsActive = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                DateCreated = DateTime.Now,
                FacultyId = facultiesList[0].Id,
                Avatar = "http://res.cloudinary.com/dlqxj0ibb/image/upload/v1712572892/avatar/user-aebb36e4-de2f-4d97-8985-199910cedb9e/y4u6isgtypvplhekz3d5.jpg",
                AvatarPublicId = "avatar/user-aebb36e4-de2f-4d97-8985-199910cedb9e/y4u6isgtypvplhekz3d5"

            },
            new()
            {
                Id = Guid.NewGuid(), FirstName = "Vu", LastName = "Nguyen", Email = "guest1@gmail.com",
                NormalizedEmail = "guest1@gmail.com".ToUpperInvariant(), UserName = "guest1",
                NormalizedUserName = "guest1".ToUpperInvariant(), IsActive = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                DateCreated = DateTime.Now,
                FacultyId = facultiesList[1].Id,
                Avatar = "http://res.cloudinary.com/dlqxj0ibb/image/upload/v1712572892/avatar/user-aebb36e4-de2f-4d97-8985-199910cedb9e/y4u6isgtypvplhekz3d5.jpg",
                AvatarPublicId = "avatar/user-aebb36e4-de2f-4d97-8985-199910cedb9e/y4u6isgtypvplhekz3d5"

            },
            new()
            {
                Id = Guid.NewGuid(), FirstName = "Hung", LastName = "Tran", Email = "guest2@gmail.com",
                NormalizedEmail = "guest2@gmail.com".ToUpperInvariant(), UserName = "guest2",
                NormalizedUserName = "guest2".ToUpperInvariant(), IsActive = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                DateCreated = DateTime.Now,
                FacultyId = facultiesList[2].Id,
                Avatar = "http://res.cloudinary.com/dlqxj0ibb/image/upload/v1712572892/avatar/user-aebb36e4-de2f-4d97-8985-199910cedb9e/y4u6isgtypvplhekz3d5.jpg",
                AvatarPublicId = "avatar/user-aebb36e4-de2f-4d97-8985-199910cedb9e/y4u6isgtypvplhekz3d5"

            },
            new()
            {
                Id = Guid.NewGuid(), FirstName = "Khang", LastName = "Nguyen", Email = "guest3@gmail.com",
                NormalizedEmail = "guest3@gmail.com".ToUpperInvariant(), UserName = "guest3",
                NormalizedUserName = "guest3".ToUpperInvariant(), IsActive = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                DateCreated = DateTime.Now,
                FacultyId = facultiesList[3].Id,
                Avatar = "http://res.cloudinary.com/dlqxj0ibb/image/upload/v1712572892/avatar/user-aebb36e4-de2f-4d97-8985-199910cedb9e/y4u6isgtypvplhekz3d5.jpg",
                AvatarPublicId = "avatar/user-aebb36e4-de2f-4d97-8985-199910cedb9e/y4u6isgtypvplhekz3d5"

            },
            new()
            {
                Id = Guid.NewGuid(), FirstName = "Khang 1", LastName = "Nguyen 1", Email = "guest4@gmail.com",
                NormalizedEmail = "guest4@gmail.com".ToUpperInvariant(), UserName = "guest4",
                NormalizedUserName = "guest4".ToUpperInvariant(), IsActive = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                DateCreated = DateTime.Now,
                FacultyId = facultiesList[4].Id,
                Avatar = "http://res.cloudinary.com/dlqxj0ibb/image/upload/v1712572892/avatar/user-aebb36e4-de2f-4d97-8985-199910cedb9e/y4u6isgtypvplhekz3d5.jpg",
                AvatarPublicId = "avatar/user-aebb36e4-de2f-4d97-8985-199910cedb9e/y4u6isgtypvplhekz3d5"

            }
        };
        foreach (var user in studentList)
        {
            user.PasswordHash = passwordHasher.HashPassword(user, "Admin123@");
        }
        foreach (var user in coordinatorList)
        {
            user.PasswordHash = passwordHasher.HashPassword(user, "Admin123@");
        }

        foreach (var user in guestList)
        {
            user.PasswordHash = passwordHasher.HashPassword(user, "Admin123@");
        }

        // Seed message
        if (!context.PrivateChats.Any() && !context.PrivateMessages.Any())
        {

            await SeedPrivateChatAndMessages(context,
                                            privateChatRepository,
                                            privateMessagesRepository,
                                            dateTimeProvider,
                                            cor_hungtkgcs200234_fpt_edu_vn,
                                             student1_gmail_com);
        }


        if (!context.Users.Any())
        {
            // admin

            var userEmail = "haodvgcs88@gmail.com";
            var userName = "admin";
            var user = new AppUser
            {
                Id = adminId,
                FirstName = "An",
                LastName = "Minh",
                Email = userEmail,
                NormalizedEmail = userEmail.ToUpperInvariant(),
                UserName = userName,
                NormalizedUserName = userName.ToUpperInvariant(),
                IsActive = true,
                // ko co cai nay ko login duoc
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                DateCreated = DateTime.Now,
                Avatar = "http://res.cloudinary.com/dlqxj0ibb/image/upload/v1712564003/avatar/user-86f43cf0-d40e-445f-a6c3-f9ff620935f0/u5ckrzjjylvr3nvhbp86.jpg",
                AvatarPublicId = "avatar/user-86f43cf0-d40e-445f-a6c3-f9ff620935f0/u5ckrzjjylvr3nvhbp86",
                FacultyId = facultiesList[1].Id,
            };
            user.PasswordHash = passwordHasher.HashPassword(user, "Admin123@");

            await context.Users.AddAsync(user);

            await context.UserRoles.AddAsync(new IdentityUserRole<Guid>
            {
                RoleId = rootAdminRoleId,
                UserId = adminId
            });
            // manager
            var managerEmail = "mushimushito2003@gmail.com";
            var managerUserName = "manager";
            var manager = new AppUser
            {
                Id = managerId,
                FirstName = "Vu",
                LastName = "Nguyen",
                Email = managerEmail,
                NormalizedEmail = managerEmail.ToUpperInvariant(),
                UserName = managerUserName,
                NormalizedUserName = managerUserName.ToUpperInvariant(),
                IsActive = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                DateCreated = DateTime.Now,
                Avatar =
                    "http://res.cloudinary.com/dlqxj0ibb/image/upload/v1712564003/avatar/user-86f43cf0-d40e-445f-a6c3-f9ff620935f0/u5ckrzjjylvr3nvhbp86.jpg",
                AvatarPublicId = "avatar/user-86f43cf0-d40e-445f-a6c3-f9ff620935f0/u5ckrzjjylvr3nvhbp86",
                FacultyId = facultiesList[1].Id,
            };
            manager.PasswordHash = passwordHasher.HashPassword(user, "Admin123@");
            await context.Users.AddAsync(manager);
            await context.UserRoles.AddAsync(new IdentityUserRole<Guid>
            {
                RoleId = managerRoleId,
                UserId = managerId
            });
            // student
            foreach (var item in studentList)
            {
                await context.Users.AddAsync(item);
                await context.UserRoles.AddAsync(new IdentityUserRole<Guid>
                {
                    RoleId = studentRoleId,
                    UserId = item.Id,
                });
            }
            // coordinator
            foreach (var item in coordinatorList)
            {
                await context.Users.AddAsync(item);
                await context.UserRoles.AddAsync(new IdentityUserRole<Guid>
                {
                    RoleId = coordinatorRoleId,
                    UserId = item.Id,
                });
            }
            //guest
            foreach (var item in guestList)
            {
                await context.Users.AddAsync(item);
                await context.UserRoles.AddAsync(new IdentityUserRole<Guid>
                {
                    RoleId = guestRoleId,
                    UserId = item.Id,
                });
            }
            await context.SaveChangesAsync();
        }

        if (context.RoleClaims.Any() == false)
        {
            // seed admin permission
            var adminPermissions = await roleManager.GetClaimsAsync(role);
            if (adminPermissions.Any() == false)
            {
                var allPermisisons = new List<RoleClaimsDto>();

                var types =
                    typeof(Permissions)
                        .GetTypeInfo()
                        .DeclaredNestedTypes
                        .ToList();

                types.ForEach(allPermisisons.GetPermissionsByType);

                foreach (var permission in allPermisisons)
                {
                    await roleManager.AddClaimAsync(role, new Claim("permissions", permission.Value!));
                }
            }
            // seed student permission
            var studentPermissions = await roleManager.GetClaimsAsync(studentRole);
            if (studentPermissions.Any() == false)
            {
                var studentPermissionList = new List<RoleClaimsDto>
                {
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.StudentDashboard.View"
                    },
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.Contributions.View"
                    },
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.Contributions.Create"
                    },
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.Contributions.Edit"
                    },
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.StudentContribution.View"
                    },
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.AddContribution.View"
                    },
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.EditContribution.View"
                    },
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.FavoriteContribution.View"
                    },
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.ReadLaterContribution.View"
                    },
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.PreviewContribution.View"
                    },
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.Contributions.Download"
                    }
                };
                foreach (var permission in studentPermissionList)
                {
                    await roleManager.AddClaimAsync(studentRole, new Claim("permissions", permission.Value!));
                }
            }
            // seed coordinator permission
            var coordinatorPermissions = await roleManager.GetClaimsAsync(coordinatorRole);
            if (coordinatorPermissions.Any() == false)
            {
                var coordinatorPermissionList = new List<RoleClaimsDto>
                {
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.Dashboard.View"
                    },
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.Contributions.View"
                    },
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.Contributions.Approve"
                    },
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.ManageContributions.View"
                    },
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.SettingGAC.View"
                    },
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.PreviewContribution.View"
                    },
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.Contributions.Download"
                    },
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.FavoriteContribution.View"
                    },
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.ReadLaterContribution.View"
                    },
                };
                foreach (var permission in coordinatorPermissionList)
                {
                    await roleManager.AddClaimAsync(coordinatorRole, new Claim("permissions", permission.Value!));
                }
            }
            // seed manager permission
            var managerPermissions = await roleManager.GetClaimsAsync(managerRole);
            if (coordinatorPermissions.Any() == false)
            {
                var managerPermissionList = new List<RoleClaimsDto>
                {
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.Dashboard.View"
                    },
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.Contributions.View"
                    },
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.ManageContributions.View"
                    },
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.ActivityLogs.View"
                    },
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.NotCommentContribution.View"
                    },
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.Contributions.Download"
                    },
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.FavoriteContribution.View"
                    },
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.ReadLaterContribution.View"
                    },
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.Users.View"
                    },
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.Faculties.View"
                    },
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.AcademicYears.View"
                    },
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.Roles.View"
                    },
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.Users.Create"
                    },
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.Users.Edit"
                    },
                };
                foreach (var permission in managerPermissionList)
                {
                    await roleManager.AddClaimAsync(managerRole, new Claim("permissions", permission.Value!));
                }
            }
            // seed guest permission
            var guestPermissions = await roleManager.GetClaimsAsync(guestRole);
            if (guestPermissions.Any() == false)
            {
                var guestPermissionList = new List<RoleClaimsDto>
                {
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.Contributions.View"
                    },
                };
                foreach (var permission in guestPermissionList)
                {
                    await roleManager.AddClaimAsync(guestRole, new Claim("permissions", permission.Value!));
                }
            }
        }

        // seed contribution
        var listContribution = new List<Contribution>
        {
            new()
            {
                AcademicYearId = yearList[0].Id,
                FacultyId = facultiesList[0].Id,
                UserId = studentList[0].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 1",
                Slug = "test-1",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[1].Id,
                FacultyId = facultiesList[0].Id,
                UserId = studentList[0].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 2",
                Slug = "test-2",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[2].Id,
                FacultyId = facultiesList[0].Id,
                UserId = studentList[0].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 3",
                Slug = "test-3",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[0].Id,
                UserId = studentList[0].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 4",
                Slug = "test-4",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[4].Id,
                FacultyId = facultiesList[0].Id,
                UserId = studentList[0].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 5",
                Slug = "test-5",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[5].Id,
                FacultyId = facultiesList[0].Id,
                UserId = studentList[0].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 6",
                Slug = "test-6",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[6].Id,
                FacultyId = facultiesList[0].Id,
                UserId = studentList[0].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 7",
                Slug = "test-7",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[7].Id,
                FacultyId = facultiesList[0].Id,
                UserId = studentList[0].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 8",
                Slug = "test-8",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[8].Id,
                FacultyId = facultiesList[0].Id,
                UserId = studentList[0].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 9",
                Slug = "test-9",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[9].Id,
                FacultyId = facultiesList[0].Id,
                UserId = studentList[0].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 10",
                Slug = "test-10",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
             new()
            {
                AcademicYearId = yearList[0].Id,
                FacultyId = facultiesList[1].Id,
                UserId = studentList[1].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 11",
                Slug = "test-11",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[1].Id,
                FacultyId = facultiesList[1].Id,
                UserId = studentList[1].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 12",
                Slug = "test-12",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
             new()
            {
                AcademicYearId = yearList[2].Id,
                FacultyId = facultiesList[1].Id,
                UserId = studentList[1].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 13",
                Slug = "test-13",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
              new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[1].Id,
                UserId = studentList[1].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 14",
                Slug = "test-14",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
               new()
            {
                AcademicYearId = yearList[4].Id,
                FacultyId = facultiesList[1].Id,
                UserId = studentList[1].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 15",
                Slug = "test-15",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
             new()
            {
                AcademicYearId = yearList[5].Id,
                FacultyId = facultiesList[1].Id,
                UserId = studentList[1].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 16",
                Slug = "test-16",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
              new()
            {
                AcademicYearId = yearList[6].Id,
                FacultyId = facultiesList[1].Id,
                UserId = studentList[1].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 17",
                Slug = "test-17",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<figure class=\"image image_resized\" style=\"width:61.75%;\" data-ckbox-resource-id=\"viPjShbsnIlO\">\r\n  <picture>\r\n      <source srcset=\"https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/103.webp 103w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/206.webp 206w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/309.webp 309w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/412.webp 412w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/515.webp 515w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/618.webp 618w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/721.webp 721w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/824.webp 824w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/927.webp 927w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/1024.webp 1024w\" sizes=\"(max-width: 1024px) 100vw, 1024px\" type=\"image/webp\"><img src=\"https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/1024.jpeg\" width=\"1024\" height=\"682\">\r\n  </picture>\r\n</figure>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`\r\n",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
              new()
            {
                AcademicYearId = yearList[7].Id,
                FacultyId = facultiesList[1].Id,
                UserId = studentList[1].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 18",
                Slug = "test-18",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
              new()
            {
                AcademicYearId = yearList[8].Id,
                FacultyId = facultiesList[1].Id,
                UserId = studentList[1].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 19",
                Slug = "test-19",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
             new()
            {
                AcademicYearId = yearList[9].Id,
                FacultyId = facultiesList[1].Id,
                UserId = studentList[1].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 20",
                Slug = "test-20",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[0].Id,
                FacultyId = facultiesList[2].Id,
                UserId =studentList[2].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 21",
                Slug = "test-21",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
             new()
            {
                AcademicYearId = yearList[1].Id,
                FacultyId = facultiesList[2].Id,
                UserId =studentList[2].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 22",
                Slug = "test-22",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
              new()
            {
                AcademicYearId = yearList[2].Id,
                FacultyId = facultiesList[2].Id,
                UserId =studentList[2].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 23",
                Slug = "test-23",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
              new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[2].Id,
                UserId =studentList[2].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 24",
                Slug = "test-24",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
               new()
            {
                AcademicYearId = yearList[4].Id,
                FacultyId = facultiesList[2].Id,
                UserId =studentList[2].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 25",
                Slug = "test-25",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
               new()
            {
                AcademicYearId = yearList[5].Id,
                FacultyId = facultiesList[2].Id,
                UserId =studentList[2].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 26",
                Slug = "test-26",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
  Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
                new()
            {
                AcademicYearId = yearList[6].Id,
                FacultyId = facultiesList[2].Id,
                UserId =studentList[2].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 27",
                Slug = "test-27",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[7].Id,
                FacultyId = facultiesList[2].Id,
                UserId =studentList[2].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 28",
                Slug = "test-28",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[8].Id,
                FacultyId = facultiesList[2].Id,
                UserId =studentList[2].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 29",
                Slug = "test-29",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
             new()
            {
                AcademicYearId = yearList[9].Id,
                FacultyId = facultiesList[2].Id,
                UserId =studentList[2].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 30",
                Slug = "test-30",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
             new()
            {
                AcademicYearId = yearList[0].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[3].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 31",
                Slug = "test-31",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",             ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},

            new()
            {
                AcademicYearId = yearList[1].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[3].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 32",
                Slug = "test-32",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",            
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
              new()
            {
                AcademicYearId = yearList[2].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[3].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 33",
                Slug = "test-33",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",            
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
              new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[3].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 34",
                Slug = "test-34",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",            
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
              new()
            {
                AcademicYearId = yearList[4].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[3].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 35",
                Slug = "test-35",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",            
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
              new()
            {
                AcademicYearId = yearList[5].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[3].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 36",
                Slug = "test-36",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",           
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
              new()
            {
                AcademicYearId = yearList[6].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[3].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 37",
                Slug = "test-37",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",            
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
              new()
            {
                AcademicYearId = yearList[7].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[3].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 38",
                Slug = "test-38",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",            
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
              new()
            {
                AcademicYearId = yearList[8].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[3].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 39",
                Slug = "test-39",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
              new()
            {
                AcademicYearId = yearList[9].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[3].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 40",
                Slug = "test-40",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",          
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
              new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[0].Id,
                UserId = studentList[0].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 41",
                Slug = "test-41",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p><meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[0].Id,
                UserId = studentList[0].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 42",
                Slug = "test-42",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[0].Id,
                UserId = studentList[0].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 43",
                Slug = "test-43",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[0].Id,
                UserId = studentList[0].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 44",
                Slug = "test-44",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[0].Id,
                UserId = studentList[0].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 45",
                Slug = "test-45",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[0].Id,
                UserId = studentList[0].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 46",
                Slug = "test-46",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[0].Id,
                UserId = studentList[0].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 47",
                Slug = "test-47",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[0].Id,
                UserId = studentList[0].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 48",
                Slug = "test-48",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[0].Id,
                UserId = studentList[0].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 49",
                Slug = "test-49",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[0].Id,
                UserId = studentList[0].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 50",
                Slug = "test-50",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
             new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[1].Id,
                UserId = studentList[1].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 51",
                Slug = "test-51",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[1].Id,
                UserId = studentList[1].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 52",
                Slug = "test-52",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
             new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[1].Id,
                UserId = studentList[1].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 53",
                Slug = "test-53",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
              new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[1].Id,
                UserId = studentList[1].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 54",
                Slug = "test-54",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
               new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[1].Id,
                UserId = studentList[1].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 55",
                Slug = "test-55",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
             new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[1].Id,
                UserId = studentList[1].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 56",
                Slug = "test-56",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
              new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[1].Id,
                UserId = studentList[1].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 57",
                Slug = "test-57",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<figure class=\"image image_resized\" style=\"width:61.75%;\" data-ckbox-resource-id=\"viPjShbsnIlO\">\r\n  <picture>\r\n      <source srcset=\"https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/103.webp 103w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/206.webp 206w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/309.webp 309w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/412.webp 412w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/515.webp 515w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/618.webp 618w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/721.webp 721w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/824.webp 824w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/927.webp 927w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/1024.webp 1024w\" sizes=\"(max-width: 1024px) 100vw, 1024px\" type=\"image/webp\"><img src=\"https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/1024.jpeg\" width=\"1024\" height=\"682\">\r\n  </picture>\r\n</figure>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`\r\n",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
              new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[1].Id,
                UserId = studentList[1].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 58",
                Slug = "test-58",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
              new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[1].Id,
                UserId = studentList[1].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 59",
                Slug = "test-59",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
             new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[1].Id,
                UserId = studentList[1].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 60",
                Slug = "test-60",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[2].Id,
                UserId =studentList[2].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 61",
                Slug = "test-61",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
             new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[2].Id,
                UserId =studentList[2].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 62",
                Slug = "test-62",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
              new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[2].Id,
                UserId =studentList[2].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 63",
                Slug = "test-63",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
              new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[2].Id,
                UserId =studentList[2].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 64",
                Slug = "test-64",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
               new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[2].Id,
                UserId =studentList[2].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 65",
                Slug = "test-65",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
               new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[2].Id,
                UserId =studentList[2].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 66",
                Slug = "test-66",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
  Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
                new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[2].Id,
                UserId =studentList[2].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 67",
                Slug = "test-67",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[2].Id,
                UserId =studentList[2].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 68",
                Slug = "test-68",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[2].Id,
                UserId =studentList[2].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 69",
                Slug = "test-69",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
             new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[2].Id,
                UserId =studentList[2].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 70",
                Slug = "test-70",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
             new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[3].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 71",
                Slug = "test-71",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",             ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},

            new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[3].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 72",
                Slug = "test-72",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
              new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[3].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 73",
                Slug = "test-73",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
              new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[3].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 74",
                Slug = "test-74",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
              new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[3].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 75",
                Slug = "test-75",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
              new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[3].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 76",
                Slug = "test-76",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
              new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[3].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 77",
                Slug = "test-77",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
              new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[3].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 78",
                Slug = "test-78",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
              new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[3].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 79",
                Slug = "test-79",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
              new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[3].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 80",
                Slug = "test-80",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
              new()
            {
                AcademicYearId = yearList[0].Id,
                FacultyId = facultiesList[0].Id,
                UserId = studentList[4].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 81",
                Slug = "test-81",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p><meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[1].Id,
                FacultyId = facultiesList[1].Id,
                UserId = studentList[5].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 82",
                Slug = "test-82",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[2].Id,
                FacultyId = facultiesList[2].Id,
                UserId = studentList[6].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 83",
                Slug = "test-83",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[7].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 84",
                Slug = "test-84",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[4].Id,
                FacultyId = facultiesList[0].Id,
                UserId = studentList[8].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 85",
                Slug = "test-85",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[5].Id,
                FacultyId = facultiesList[1].Id,
                UserId = studentList[9].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 86",
                Slug = "test-86",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[6].Id,
                FacultyId = facultiesList[0].Id,
                UserId = studentList[4].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 87",
                Slug = "test-87",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[7].Id,
                FacultyId = facultiesList[1].Id,
                UserId = studentList[5].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 88",
                Slug = "test-88",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[8].Id,
                FacultyId = facultiesList[2].Id,
                UserId = studentList[6].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 89",
                Slug = "test-89",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[9].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[7].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 90",
                Slug = "test-90",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
             new()
            {
                AcademicYearId = yearList[0].Id,
                FacultyId = facultiesList[0].Id,
                UserId = studentList[8].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 91",
                Slug = "test-91",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[1].Id,
                FacultyId = facultiesList[1].Id,
                UserId = studentList[9].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 92",
                Slug = "test-92",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
             new()
            {
                AcademicYearId = yearList[2].Id,
                FacultyId = facultiesList[0].Id,
                UserId = studentList[4].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 93",
                Slug = "test-93",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
              new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[1].Id,
                UserId = studentList[5].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 94",
                Slug = "test-94",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
               new()
            {
                AcademicYearId = yearList[4].Id,
                FacultyId = facultiesList[2].Id,
                UserId = studentList[6].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 95",
                Slug = "test-95",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
             new()
            {
                AcademicYearId = yearList[5].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[7].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 96",
                Slug = "test-96",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
              new()
            {
                AcademicYearId = yearList[6].Id,
                FacultyId = facultiesList[0].Id,
                UserId = studentList[8].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 97",
                Slug = "test-97",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<figure class=\"image image_resized\" style=\"width:61.75%;\" data-ckbox-resource-id=\"viPjShbsnIlO\">\r\n  <picture>\r\n      <source srcset=\"https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/103.webp 103w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/206.webp 206w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/309.webp 309w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/412.webp 412w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/515.webp 515w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/618.webp 618w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/721.webp 721w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/824.webp 824w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/927.webp 927w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/1024.webp 1024w\" sizes=\"(max-width: 1024px) 100vw, 1024px\" type=\"image/webp\"><img src=\"https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/1024.jpeg\" width=\"1024\" height=\"682\">\r\n  </picture>\r\n</figure>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`\r\n",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
              new()
            {
                AcademicYearId = yearList[7].Id,
                FacultyId = facultiesList[1].Id,
                UserId = studentList[9].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 98",
                Slug = "test-98",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
              new()
            {
                AcademicYearId = yearList[8].Id,
                FacultyId = facultiesList[1].Id,
                UserId = studentList[1].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 99",
                Slug = "test-99",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
             new()
            {
                AcademicYearId = yearList[9].Id,
                FacultyId = facultiesList[1].Id,
                UserId = studentList[1].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 100",
                Slug = "test-100",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[0].Id,
                FacultyId = facultiesList[2].Id,
                UserId =studentList[2].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 101",
                Slug = "test-101",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
             new()
            {
                AcademicYearId = yearList[1].Id,
                FacultyId = facultiesList[2].Id,
                UserId =studentList[2].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 102",
                Slug = "test-102",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
              new()
            {
                AcademicYearId = yearList[2].Id,
                FacultyId = facultiesList[2].Id,
                UserId =studentList[2].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 103",
                Slug = "test-103",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
              new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[2].Id,
                UserId =studentList[2].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 104",
                Slug = "test-104",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
               new()
            {
                AcademicYearId = yearList[4].Id,
                FacultyId = facultiesList[2].Id,
                UserId =studentList[2].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 105",
                Slug = "test-105",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
               new()
            {
                AcademicYearId = yearList[5].Id,
                FacultyId = facultiesList[2].Id,
                UserId =studentList[2].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 106",
                Slug = "test-106",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
  Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
                new()
            {
                AcademicYearId = yearList[6].Id,
                FacultyId = facultiesList[2].Id,
                UserId =studentList[2].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 107",
                Slug = "test-107",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[7].Id,
                FacultyId = facultiesList[2].Id,
                UserId =studentList[2].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 108",
                Slug = "test-108",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[8].Id,
                FacultyId = facultiesList[2].Id,
                UserId =studentList[2].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 109",
                Slug = "test-109",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
             new()
            {
                AcademicYearId = yearList[9].Id,
                FacultyId = facultiesList[2].Id,
                UserId =studentList[2].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 110",
                Slug = "test-110",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
             new()
            {
                AcademicYearId = yearList[0].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[3].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 111",
                Slug = "test-111",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",             ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},

            new()
            {
                AcademicYearId = yearList[1].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[3].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 112",
                Slug = "test-112",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
              new()
            {
                AcademicYearId = yearList[2].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[3].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 113",
                Slug = "test-113",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
              new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[3].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 114",
                Slug = "test-114",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
              new()
            {
                AcademicYearId = yearList[4].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[3].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 115",
                Slug = "test-115",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
              new()
            {
                AcademicYearId = yearList[5].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[3].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 116",
                Slug = "test-116",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
              new()
            {
                AcademicYearId = yearList[6].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[3].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 117",
                Slug = "test-117",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
              new()
            {
                AcademicYearId = yearList[7].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[3].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 118",
                Slug = "test-118",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
              new()
            {
                AcademicYearId = yearList[8].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[3].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 119",
                Slug = "test-119",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
              new()
            {
                AcademicYearId = yearList[9].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[3].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 120",
                Slug = "test-120",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
               new()
            {
                AcademicYearId = yearList[9].Id,
                FacultyId = facultiesList[4].Id,
                UserId = studentList[10].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 121",
                Slug = "test-121",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[0].Id,
                FacultyId = facultiesList[4].Id,
                UserId = studentList[10].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 122",
                Slug = "test-122",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
             new()
            {
                AcademicYearId = yearList[1].Id,
                FacultyId = facultiesList[4].Id,
                UserId = studentList[10].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 123",
                Slug = "test-123",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
              new()
            {
                AcademicYearId = yearList[2].Id,
                FacultyId = facultiesList[4].Id,
                UserId = studentList[10].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 124",
                Slug = "test-124",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
              new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[4].Id,
                UserId = studentList[10].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 125",
                Slug = "test-125",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
               new()
            {
                AcademicYearId = yearList[4].Id,
                FacultyId = facultiesList[4].Id,
                UserId = studentList[10].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 126",
                Slug = "test-126",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
               new()
            {
                AcademicYearId = yearList[5].Id,
                FacultyId = facultiesList[4].Id,
                UserId = studentList[10].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 127",
                Slug = "test-127",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
  Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
                new()
            {
                AcademicYearId = yearList[6].Id,
                FacultyId = facultiesList[4].Id,
                UserId = studentList[10].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 128",
                Slug = "test-128",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[7].Id,
                FacultyId = facultiesList[4].Id,
                UserId = studentList[10].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 129",
                Slug = "test-129",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
            new()
            {
                AcademicYearId = yearList[8].Id,
                FacultyId = facultiesList[4].Id,
                UserId = studentList[10].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 130",
                Slug = "test-130",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
             new()
            {
                AcademicYearId = yearList[9].Id,
                FacultyId = facultiesList[4].Id,
                UserId = studentList[10].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 131",
                Slug = "test-131",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",                 ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"
            },
             new()
            {
                AcademicYearId = yearList[0].Id,
                FacultyId = facultiesList[4].Id,
                UserId = studentList[10].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 132",
                Slug = "test-132",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",             ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},

            new()
            {
                AcademicYearId = yearList[1].Id,
                FacultyId = facultiesList[4].Id,
                UserId = studentList[10].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 133",
                Slug = "test-133",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
              new()
            {
                AcademicYearId = yearList[2].Id,
                FacultyId = facultiesList[4].Id,
                UserId = studentList[10].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 134",
                Slug = "test-134",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
              new()
            {
                AcademicYearId = yearList[3].Id,
                FacultyId = facultiesList[4].Id,
                UserId = studentList[10].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 135",
                Slug = "test-135",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
              new()
            {
                AcademicYearId = yearList[4].Id,
                FacultyId = facultiesList[4].Id,
                UserId = studentList[10].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 136",
                Slug = "test-136",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
              new()
            {
                AcademicYearId = yearList[5].Id,
                FacultyId = facultiesList[4].Id,
                UserId = studentList[10].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 137",
                Slug = "test-137",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
              new()
            {
                AcademicYearId = yearList[6].Id,
                FacultyId = facultiesList[4].Id,
                UserId = studentList[10].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 138",
                Slug = "test-138",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
              new()
            {
                AcademicYearId = yearList[7].Id,
                FacultyId = facultiesList[4].Id,
                UserId = studentList[10].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 139",
                Slug = "test-139",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
              new()
            {
                AcademicYearId = yearList[8].Id,
                FacultyId = facultiesList[4].Id,
                UserId = studentList[10].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 140",
                Slug = "test-140",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
              new()
            {
                AcademicYearId = yearList[9].Id,
                FacultyId = facultiesList[4].Id,
                UserId = studentList[10].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 141",
                Slug = "test-141",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
                Content = "<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style=\"white-space:pre-wrap;\">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style=\"white-space:pre-wrap;\">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n  </strong><span style=\"white-space:pre-wrap;\"><strong></strong></span>\r\n</p>\r\n<p>\r\n  <meta charset=\"utf-8\"><span data-metadata=\"\"></span><span data-buffer=\"\"></span>\r\n</p>\r\n<p><span style=\"white-space:pre-wrap;\">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`",
                ShortDescription = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.\r\n          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.\r\n"},
        };
        if (!context.Contributions.Any())
        {
            foreach (var contribution in listContribution)
            {
                await context.Contributions.AddAsync(contribution);

            }
            await context.SaveChangesAsync();
            foreach (var contribution in listContribution)
            {
                await contributionRepository.SendToApprove(contribution.Id, adminId);
            }
            await context.SaveChangesAsync();
        }
        // add file 
        if (!context.Files.Any())
        {
            for (var i = 0; i <= 39; i++)
            {
                await context.Files.AddRangeAsync(new List<File>
                {
                    new()
                    {
                        ContributionId = listContribution[i].Id, DateCreated = DateTime.UtcNow, Id = Guid.NewGuid(),
                        Path = "http://res.cloudinary.com/dlqxj0ibb/image/upload/v1713881334/thumbnail/contribution-0c4a2cd0-5df5-4eed-b9a9-88aff280a8ab/jgbzedqwhe5xbogfrpgj.png",
                        Name = "default.png", Type = FileType.Thumbnail,
                        PublicId = "thumbnail/contribution-0c4a2cd0-5df5-4eed-b9a9-88aff280a8ab/jgbzedqwhe5xbogfrpgj",
                        Extension = ".png"
                    },
                    new()
                    {
                        ContributionId = listContribution[i].Id, DateCreated = DateTime.UtcNow, Id = Guid.NewGuid(),
                        Path =
                            "http://res.cloudinary.com/dlqxj0ibb/raw/upload/v1712572059/file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/cd2vojbncylsqljk0xww.docx",
                        Name = "file asm.docx", Type = FileType.File,
                        PublicId = "file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/cd2vojbncylsqljk0xww.docx",
                        Extension = ".docx"
                    },
                    new()
                    {
                        ContributionId = listContribution[i].Id, DateCreated = DateTime.UtcNow, Id = Guid.NewGuid(),
                        Path =
                            "http://res.cloudinary.com/dlqxj0ibb/raw/upload/v1712572059/file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/dj5kco9ywflongrrsd4h.docx",
                        Name = "1_Unit 1 - Assignment 1 frontsheet.docx", Type = FileType.File,
                        PublicId = "file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/dj5kco9ywflongrrsd4h.docx",
                        Extension = ".docx"
                    },
                    new()
                    {
                        ContributionId = listContribution[i].Id, DateCreated = DateTime.UtcNow, Id = Guid.NewGuid(),
                        Path =
                            "http://res.cloudinary.com/dlqxj0ibb/raw/upload/v1712572060/file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/i8mhjnipdm8ozmjkwl1d.docx",
                        Name = "1_Unit 1 - Assignment 2 frontsheet_.docx    ", Type = FileType.File,
                        PublicId = "file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/i8mhjnipdm8ozmjkwl1d.docx",
                        Extension = ".docx"
                    },
                    new()
                    {
                        ContributionId = listContribution[i].Id, DateCreated = DateTime.UtcNow, Id = Guid.NewGuid(),
                        Path =
                            "http://res.cloudinary.com/dlqxj0ibb/raw/upload/v1712572061/file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/niip3irfujngx2l8yxnl.docx",
                        Name = "TCS2405-group1.docx", Type = FileType.File,
                        PublicId = "file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/niip3irfujngx2l8yxnl.docx",
                        Extension = ".docx"
                    },
                    new()
                    {
                        ContributionId = listContribution[i].Id, DateCreated = DateTime.UtcNow, Id = Guid.NewGuid(),
                        Path =
                            "http://res.cloudinary.com/dlqxj0ibb/raw/upload/v1712572061/file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/qu5j8gxtyjffvxskcjwv.docx",
                        Name = "TCS2405 - group 2.docx", Type = FileType.File,
                        PublicId = "file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/qu5j8gxtyjffvxskcjwv.docx",
                        Extension = ".docx"
                    },
                });
            }
            for (var i = 40; i <= 89; i++)
            {
                await context.Files.AddRangeAsync(new List<File>
                {
                    new()
                    {
                        ContributionId = listContribution[i].Id, DateCreated = DateTime.UtcNow, Id = Guid.NewGuid(),
                        Path = "http://res.cloudinary.com/dlqxj0ibb/image/upload/v1713881246/thumbnail/contribution-bf199c59-137c-40b3-9f7a-44e5e8f82aea/bovn6whxcw1uvq34hiup.jpg",
                        Name = "default.png", Type = FileType.Thumbnail,
                        PublicId = "thumbnail/contribution-bf199c59-137c-40b3-9f7a-44e5e8f82aea/bovn6whxcw1uvq34hiup",
                        Extension = ".png"
                    },
                    new()
                    {
                        ContributionId = listContribution[i].Id, DateCreated = DateTime.UtcNow, Id = Guid.NewGuid(),
                        Path =
                            "http://res.cloudinary.com/dlqxj0ibb/raw/upload/v1712572059/file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/cd2vojbncylsqljk0xww.docx",
                        Name = "file asm.docx", Type = FileType.File,
                        PublicId = "file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/cd2vojbncylsqljk0xww.docx",
                        Extension = ".docx"
                    },
                    new()
                    {
                        ContributionId = listContribution[i].Id, DateCreated = DateTime.UtcNow, Id = Guid.NewGuid(),
                        Path =
                            "http://res.cloudinary.com/dlqxj0ibb/raw/upload/v1712572059/file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/dj5kco9ywflongrrsd4h.docx",
                        Name = "1_Unit 1 - Assignment 1 frontsheet.docx", Type = FileType.File,
                        PublicId = "file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/dj5kco9ywflongrrsd4h.docx",
                        Extension = ".docx"
                    },
                    new()
                    {
                        ContributionId = listContribution[i].Id, DateCreated = DateTime.UtcNow, Id = Guid.NewGuid(),
                        Path =
                            "http://res.cloudinary.com/dlqxj0ibb/raw/upload/v1712572060/file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/i8mhjnipdm8ozmjkwl1d.docx",
                        Name = "1_Unit 1 - Assignment 2 frontsheet_.docx    ", Type = FileType.File,
                        PublicId = "file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/i8mhjnipdm8ozmjkwl1d.docx",
                        Extension = ".docx"
                    },
                    new()
                    {
                        ContributionId = listContribution[i].Id, DateCreated = DateTime.UtcNow, Id = Guid.NewGuid(),
                        Path =
                            "http://res.cloudinary.com/dlqxj0ibb/raw/upload/v1712572061/file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/niip3irfujngx2l8yxnl.docx",
                        Name = "TCS2405-group1.docx", Type = FileType.File,
                        PublicId = "file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/niip3irfujngx2l8yxnl.docx",
                        Extension = ".docx"
                    },
                    new()
                    {
                        ContributionId = listContribution[i].Id, DateCreated = DateTime.UtcNow, Id = Guid.NewGuid(),
                        Path =
                            "http://res.cloudinary.com/dlqxj0ibb/raw/upload/v1712572061/file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/qu5j8gxtyjffvxskcjwv.docx",
                        Name = "TCS2405 - group 2.docx", Type = FileType.File,
                        PublicId = "file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/qu5j8gxtyjffvxskcjwv.docx",
                        Extension = ".docx"
                    },
                });
            }
            for (var i = 90; i <= 119; i++)
            {
                await context.Files.AddRangeAsync(new List<File>
                {
                    new()
                    {
                        ContributionId = listContribution[i].Id, DateCreated = DateTime.UtcNow, Id = Guid.NewGuid(),
                        Path = "http://res.cloudinary.com/dlqxj0ibb/image/upload/v1713881200/thumbnail/contribution-5fd660a4-d651-4024-8512-a9b776bb1f51/gqalrr1ghlpmyb7br2fv.jpg",
                        Name = "default.png", Type = FileType.Thumbnail,
                        PublicId = "thumbnail/contribution-5fd660a4-d651-4024-8512-a9b776bb1f51/gqalrr1ghlpmyb7br2fv",
                        Extension = ".png"
                    },
                    new()
                    {
                        ContributionId = listContribution[i].Id, DateCreated = DateTime.UtcNow, Id = Guid.NewGuid(),
                        Path =
                            "http://res.cloudinary.com/dlqxj0ibb/raw/upload/v1712572059/file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/cd2vojbncylsqljk0xww.docx",
                        Name = "file asm.docx", Type = FileType.File,
                        PublicId = "file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/cd2vojbncylsqljk0xww.docx",
                        Extension = ".docx"
                    },
                    new()
                    {
                        ContributionId = listContribution[i].Id, DateCreated = DateTime.UtcNow, Id = Guid.NewGuid(),
                        Path =
                            "http://res.cloudinary.com/dlqxj0ibb/raw/upload/v1712572059/file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/dj5kco9ywflongrrsd4h.docx",
                        Name = "1_Unit 1 - Assignment 1 frontsheet.docx", Type = FileType.File,
                        PublicId = "file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/dj5kco9ywflongrrsd4h.docx",
                        Extension = ".docx"
                    },
                    new()
                    {
                        ContributionId = listContribution[i].Id, DateCreated = DateTime.UtcNow, Id = Guid.NewGuid(),
                        Path =
                            "http://res.cloudinary.com/dlqxj0ibb/raw/upload/v1712572060/file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/i8mhjnipdm8ozmjkwl1d.docx",
                        Name = "1_Unit 1 - Assignment 2 frontsheet_.docx    ", Type = FileType.File,
                        PublicId = "file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/i8mhjnipdm8ozmjkwl1d.docx",
                        Extension = ".docx"
                    },
                    new()
                    {
                        ContributionId = listContribution[i].Id, DateCreated = DateTime.UtcNow, Id = Guid.NewGuid(),
                        Path =
                            "http://res.cloudinary.com/dlqxj0ibb/raw/upload/v1712572061/file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/niip3irfujngx2l8yxnl.docx",
                        Name = "TCS2405-group1.docx", Type = FileType.File,
                        PublicId = "file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/niip3irfujngx2l8yxnl.docx",
                        Extension = ".docx"
                    },
                    new()
                    {
                        ContributionId = listContribution[i].Id, DateCreated = DateTime.UtcNow, Id = Guid.NewGuid(),
                        Path =
                            "http://res.cloudinary.com/dlqxj0ibb/raw/upload/v1712572061/file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/qu5j8gxtyjffvxskcjwv.docx",
                        Name = "TCS2405 - group 2.docx", Type = FileType.File,
                        PublicId = "file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/qu5j8gxtyjffvxskcjwv.docx",
                        Extension = ".docx"
                    },
                });
            }
            //foreach (var contribution in listContribution)
            //{
            //    await context.Files.AddRangeAsync(new List<File>
            //    {
            //        new()
            //        {
            //            ContributionId = contribution.Id, DateCreated = DateTime.UtcNow, Id = Guid.NewGuid(),
            //            Path = "https://variety.com/wp-content/uploads/2021/04/Avatar.jpg",
            //            Name = "default.png", Type = FileType.Thumbnail,
            //            PublicId = "thumbnail/contribution-c3027ba5-3794-4ea3-9719-8ea18a4c2d10/qutiwdhpg6zfme2nfqce",
            //            Extension = ".png"
            //        },
            //        new()
            //        {
            //            ContributionId = contribution.Id, DateCreated = DateTime.UtcNow, Id = Guid.NewGuid(),
            //            Path = "http://res.cloudinary.com/dlqxj0ibb/raw/upload/v1712572059/file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/cd2vojbncylsqljk0xww.docx",
            //            Name = "file asm.docx", Type = FileType.File,
            //            PublicId = "file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/cd2vojbncylsqljk0xww.docx",
            //            Extension = ".docx"
            //        },
            //        new()
            //        {
            //            ContributionId = contribution.Id, DateCreated = DateTime.UtcNow, Id = Guid.NewGuid(),
            //            Path = "http://res.cloudinary.com/dlqxj0ibb/raw/upload/v1712572059/file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/dj5kco9ywflongrrsd4h.docx",
            //            Name = "1_Unit 1 - Assignment 1 frontsheet.docx", Type = FileType.File,
            //            PublicId = "file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/dj5kco9ywflongrrsd4h.docx",
            //            Extension = ".docx"
            //        },
            //        new()
            //        {
            //            ContributionId = contribution.Id, DateCreated = DateTime.UtcNow, Id = Guid.NewGuid(),
            //            Path = "http://res.cloudinary.com/dlqxj0ibb/raw/upload/v1712572060/file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/i8mhjnipdm8ozmjkwl1d.docx",
            //            Name = "1_Unit 1 - Assignment 2 frontsheet_.docx    ", Type = FileType.File,
            //            PublicId = "file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/i8mhjnipdm8ozmjkwl1d.docx",
            //            Extension = ".docx"
            //        },
            //        new()
            //        {
            //            ContributionId = contribution.Id, DateCreated = DateTime.UtcNow, Id = Guid.NewGuid(),
            //            Path = "http://res.cloudinary.com/dlqxj0ibb/raw/upload/v1712572061/file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/niip3irfujngx2l8yxnl.docx",
            //            Name = "TCS2405-group1.docx", Type = FileType.File,
            //            PublicId = "file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/niip3irfujngx2l8yxnl.docx",
            //            Extension = ".docx"
            //        },
            //        new()
            //        {
            //            ContributionId = contribution.Id, DateCreated = DateTime.UtcNow, Id = Guid.NewGuid(),
            //            Path = "http://res.cloudinary.com/dlqxj0ibb/raw/upload/v1712572061/file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/qu5j8gxtyjffvxskcjwv.docx",
            //            Name = "TCS2405 - group 2.docx", Type = FileType.File,
            //            PublicId = "file/contribution-f99a2fcc-62da-48b0-bad6-f51c4af0d9e2/qu5j8gxtyjffvxskcjwv.docx",
            //            Extension = ".docx"
            //        },
            //    });
            //}
        }
        // approve
        if (!context.ContributionPublics.Any())
        {
            // await contributionRepository.Approve(listContribution[0], adminId);
            // await contributionRepository.Approve(listContribution[1], adminId);
            // await contributionRepository.Approve(listContribution[2], adminId);
            // await contributionRepository.Approve(listContribution[3], adminId);
            // await contributionRepository.Approve(listContribution[4], adminId);
            // await contributionRepository.Approve(listContribution[5], adminId);
            // await contributionRepository.Approve(listContribution[6], adminId);
            // await contributionRepository.Approve(listContribution[7], adminId);
            // await contributionRepository.Approve(listContribution[8], adminId);
            // await contributionRepository.Approve(listContribution[9], adminId);
            // await contributionRepository.Approve(listContribution[10], adminId);
            // await contributionRepository.Approve(listContribution[11], adminId);
            // await contributionRepository.Approve(listContribution[12], adminId);
            // await contributionRepository.Approve(listContribution[13], adminId);
            // await contributionRepository.Approve(listContribution[14], adminId);
            // await contributionRepository.Approve(listContribution[15], adminId);
            // await contributionRepository.Approve(listContribution[16], adminId);
            // await contributionRepository.Approve(listContribution[17], adminId);
            // await contributionRepository.Approve(listContribution[18], adminId);
            // await contributionRepository.Approve(listContribution[19], adminId);
            for (var i = 0; i <= 5; i++)
            {
                await contributionRepository.Reject(listContribution[i], adminId, "default reject");
            }
            for (var i = 10; i <= 59; i++)
            {
                await contributionRepository.Approve(listContribution[i], adminId);
            }

            for (var i = 60; i <= 99; i++)
            {
                await contributionRepository.Reject(listContribution[i], adminId,"default reject");
            }
            await context.SaveChangesAsync();
        }
        // seed comment
        if (!context.ContributionPublicComments.Any())
        {
            for (var i = 10;i<=39;i++)
            {
                context.ContributionPublicComments.Add(new ContributionPublicComment()
                {
                    Content = $"test comment {i}",
                    ContributionId = listContribution[i].Id,
                    UserId = studentList[1].Id,
                });
                context.ContributionPublicComments.Add(new ContributionPublicComment()
                {
                    Content = $"test comment {i}",
                    ContributionId = listContribution[i].Id,
                    UserId = studentList[2].Id,
                });
                context.ContributionPublicComments.Add(new ContributionPublicComment()
                {
                    Content = $"test comment {i}",
                    ContributionId = listContribution[i].Id,
                    UserId = studentList[3].Id,
                });

            }
            for (var i = 40; i <= 59; i++)
            {
                context.ContributionPublicComments.Add(new ContributionPublicComment()
                {
                    Content = $"test comment {i}",
                    ContributionId = listContribution[i].Id,
                    UserId = studentList[0].Id,
                });
                context.ContributionPublicComments.Add(new ContributionPublicComment()
                {
                    Content = $"test comment {i}",
                    ContributionId = listContribution[i].Id,
                    UserId = studentList[2].Id,
                });
                context.ContributionPublicComments.Add(new ContributionPublicComment()
                {
                    Content = $"test comment {i}",
                    ContributionId = listContribution[i].Id,
                    UserId = studentList[3].Id,
                });
                context.ContributionPublicComments.Add(new ContributionPublicComment()
                {
                    Content = $"test comment {i}",
                    ContributionId = listContribution[i].Id,
                    UserId = studentList[4].Id,
                });
                context.ContributionPublicComments.Add(new ContributionPublicComment()
                {
                    Content = $"test comment {i}",
                    ContributionId = listContribution[i].Id,
                    UserId = studentList[5].Id,
                });
                context.ContributionPublicComments.Add(new ContributionPublicComment()
                {
                    Content = $"test comment {i}",
                    ContributionId = listContribution[i].Id,
                    UserId = studentList[6].Id,
                });
                context.ContributionPublicComments.Add(new ContributionPublicComment()
                {
                    Content = $"test comment {i}",
                    ContributionId = listContribution[i].Id,
                    UserId = studentList[7].Id,
                });
                context.ContributionPublicComments.Add(new ContributionPublicComment()
                {
                    Content = $"test comment {i}",
                    ContributionId = listContribution[i].Id,
                    UserId = studentList[1].Id,
                });
            }
           
            await context.SaveChangesAsync();
        }
        // seed like
        if (!context.Likes.Any())
        {
            Random rnd = new Random();
            for (var i = 10; i <= 39; i++)
            {


                var contribution = await context.ContributionPublics.FindAsync(listContribution[i].Id);
                context.Likes.Add(new Like()
                {

                    UserId = studentList[1].Id,
                    ContributionPublicId = listContribution[i].Id
                });
                contribution.LikeQuantity += 1;
                context.Likes.Add(new Like()
                {

                    UserId = studentList[2].Id,
                    ContributionPublicId = listContribution[i].Id
                });
                contribution.LikeQuantity += 1;
                context.Likes.Add(new Like()
                {

                    UserId = studentList[3].Id,
                    ContributionPublicId = listContribution[i].Id
                });
                contribution.LikeQuantity += 1;
                contribution.Views = rnd.Next(1, 50);
            }
            for (var i = 40; i <= 59; i++)
            {
                var contribution = await context.ContributionPublics.FindAsync(listContribution[i].Id);
                context.Likes.Add(new Like()
                {

                    UserId = studentList[0].Id,
                    ContributionPublicId = listContribution[i].Id
                });
                contribution.LikeQuantity += 1;
                //context.Likes.Add(new Like()
                //{

                //    UserId = studentList[2].Id,
                //    ContributionPublicId = listContribution[i].Id
                //});
                //contribution.LikeQuantity += 1;
                context.Likes.Add(new Like()
                {

                    UserId = studentList[3].Id,
                    ContributionPublicId = listContribution[i].Id
                });
                contribution.LikeQuantity += 1;
                contribution.Views = rnd.Next(1, 50);
            }
        
            await context.SaveChangesAsync();
        }
        // seed rating
        if (!context.ContributionPublicRatings.Any())
        {
            for (var i = 10; i <= 39; i++)
            {
                var contribution = await context.ContributionPublics.FindAsync(listContribution[i].Id);

                context.ContributionPublicRatings.Add(new ContributionPublicRating()
                {

                    UserId = studentList[1].Id,
                    ContributionPublicId = listContribution[i].Id,
                    Rating = 5
                });
                context.ContributionPublicRatings.Add(new ContributionPublicRating()
                {

                    UserId = studentList[2].Id,
                    ContributionPublicId = listContribution[i].Id,
                    Rating = 4
                });
                context.ContributionPublicRatings.Add(new ContributionPublicRating()
                {

                    UserId = studentList[3].Id,
                    ContributionPublicId = listContribution[i].Id,
                    Rating = 5
                });
                context.ContributionPublicRatings.Add(new ContributionPublicRating()
                {

                    UserId = studentList[4].Id,
                    ContributionPublicId = listContribution[i].Id,
                    Rating = 3
                });
                context.ContributionPublicRatings.Add(new ContributionPublicRating()
                {

                    UserId = studentList[5].Id,
                    ContributionPublicId = listContribution[i].Id,
                    Rating = 5
                });
                await context.SaveChangesAsync();
                contribution.AverageRating = await ratingRepository.GetAverageRatingAsync(contribution.Id);
                await context.SaveChangesAsync();
            }
            for (var i = 40; i <= 59; i++)
            {
                var contribution = await context.ContributionPublics.FindAsync(listContribution[i].Id);
                context.ContributionPublicRatings.Add(new ContributionPublicRating()
                {

                    UserId = studentList[1].Id,
                    ContributionPublicId = listContribution[i].Id,
                    Rating = 5
                });
                context.ContributionPublicRatings.Add(new ContributionPublicRating()
                {

                    UserId = studentList[2].Id,
                    ContributionPublicId = listContribution[i].Id,
                    Rating = 4
                });
                context.ContributionPublicRatings.Add(new ContributionPublicRating()
                {

                    UserId = studentList[3].Id,
                    ContributionPublicId = listContribution[i].Id,
                    Rating = 5
                });
                context.ContributionPublicRatings.Add(new ContributionPublicRating()
                {

                    UserId = studentList[6].Id,
                    ContributionPublicId = listContribution[i].Id,
                    Rating = 4
                });
                context.ContributionPublicRatings.Add(new ContributionPublicRating()
                {

                    UserId = studentList[7].Id,
                    ContributionPublicId = listContribution[i].Id,
                    Rating = 5
                });
                await context.SaveChangesAsync();
                contribution.AverageRating = await ratingRepository.GetAverageRatingAsync(contribution.Id);
                await context.SaveChangesAsync();
            }
        }
        //if (!context.ContributionComments.Any())
        //{
        //    foreach (var item in listContribution)
        //    {

        //        context.ContributionComments.Add(new ContributionComment
        //        {
        //            Content = "test from student",
        //            ContributionId = item.Id,
        //            UserId = studentList[1].Id,
        //        });
        //        context.ContributionComments.Add(new ContributionComment
        //        {
        //            Content = "test from coordinator",
        //            ContributionId = item.Id,
        //            UserId = coordinatorList[1].Id,
        //        });
        //    }
        //    await context.SaveChangesAsync();
        //}
    }
    public static async Task SeedFaculty(AppDbContext context,
                                         IFacultyRepository facultyRepository)
    {
        var allFaculties = new List<string>
        {
            Faculties.Bussiness,
            Faculties.IT,
            Faculties.Design,
            Faculties.Marketing,
        };

        foreach (var facultyName in allFaculties)
        {
            if (await facultyRepository.GetFacultyByName(facultyName) is null)
            {
                facultyRepository.Add(new Faculty
                {
                    Name = facultyName,
                    Icon = "DefaultIcon",
                });
            }
        }

        await context.SaveChangesAsync();
    }
    public static async Task SeedAcademicYear(AppDbContext context,
        IAcademicYearRepository academicYearRepository)
    {
        var academicYears = new List<AcademicYear>
        {
            new AcademicYear { Name = "2021-2022", StartClosureDate = new DateTime(2022, 5, 1), EndClosureDate = new DateTime(2022, 7, 1), FinalClosureDate = new DateTime(2022, 9, 1), UserNameCreated = "Seeder" },
            new AcademicYear { Name = "2022-2023", StartClosureDate = new DateTime(2023, 5, 1), EndClosureDate = new DateTime(2023, 7, 1), FinalClosureDate = new DateTime(2023, 9, 1), UserNameCreated = "Seeder" },

        };

        foreach (var academicYear in academicYears)
        {
            if (await academicYearRepository.GetAcademicYearByName(academicYear.Name) is null)
            {
                academicYearRepository.Add(academicYear);
            }
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedPrivateChatAndMessages(AppDbContext dbContext,
                                                        IPrivateChatRepository privateChatRepository,
                                                        IPrivateMessagesRepository privateMessagesRepository,
                                                         IDateTimeProvider dateTimeProvider,
                                                         Guid corId,
                                                         Guid stdId)
    {


        privateChatRepository.Add(new PrivateChat
        {
            User1Id = corId,
            User2Id = stdId,
            LastActivity = dateTimeProvider.UtcNow,
        });

        privateMessagesRepository.Add(new PrivateMessage
        {
            ChatId = Guid.NewGuid(),
            SenderId = corId,
            ReceiverId = stdId,
            Content = "Hallo",
        });

        privateMessagesRepository.Add(new PrivateMessage
        {
            ChatId = Guid.NewGuid(),
            SenderId = corId,
            ReceiverId = stdId,
            Content = "Um wie viel Uhr gehen Sie die Schule?",
        });

        privateMessagesRepository.Add(new PrivateMessage
        {
            ChatId = Guid.NewGuid(),
            SenderId = stdId,
            ReceiverId = corId,
            Content = "Um halb neunzig Uhr",
        });

        await dbContext.SaveChangesAsync();
    }
}