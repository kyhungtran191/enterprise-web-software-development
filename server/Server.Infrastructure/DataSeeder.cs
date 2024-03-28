using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Server.Application.Common.Dtos;
using Server.Application.Common.Extensions;
using Server.Application.Common.Interfaces.Persistence;
using Server.Domain.Common.Constants;
using Server.Domain.Entity.Content;
using Server.Domain.Entity.Identity;
using Server.Infrastructure.Persistence.Repositories;

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

    public static async Task SeedContribution(AppDbContext context, RoleManager<AppRole> roleManager)
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
                Name = "2021-2022", StartClosureDate = new DateTime(2022, 5, 1),
                EndClosureDate = new DateTime(2022, 7, 1), FinalClosureDate = new DateTime(2022, 9, 1),
                UserNameCreated = "Default",DateCreated = DateTime.Now
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "2022-2023", StartClosureDate = new DateTime(2023, 5, 1),
                EndClosureDate = new DateTime(2023, 7, 1), FinalClosureDate = new DateTime(2023, 9, 1),
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
        var userId = Guid.NewGuid();

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

        if (context.Users.Count() < 2)
        {
            var userEmail = "nguahoang2003@gmail.com";
            var userName = "admin2";
            var user = new AppUser
            {
                Id = userId,
                FirstName = "Vu",
                LastName = "Nguyen",
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

        // seed contribution
        var listContribution = new List<Contribution>
        {
            new()
            {
                AcademicYearId = yearList[0].Id,
                FacultyId = facultiesList[0].Id,
                UserId = userId,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 1",
                Slug = "test-1",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
            },
            new()
            {
                AcademicYearId = yearList[1].Id,
                FacultyId = facultiesList[1].Id,
                UserId = userId,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 2",
                Slug = "test-2",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
            },
            new()
            {
                AcademicYearId = yearList[0].Id,
                FacultyId = facultiesList[2].Id,
                UserId = userId,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 3",
                Slug = "test-3",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
            },
            new()
            {
                AcademicYearId = yearList[1].Id,
                FacultyId = facultiesList[3].Id,
                UserId = userId,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 4",
                Slug = "test-4",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
            }
        };
        if (!context.Contributions.Any())
        {
            foreach (var contribution in listContribution)
            {
                await context.Contributions.AddAsync(contribution);
            }
            await context.SaveChangesAsync();
        }
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

}