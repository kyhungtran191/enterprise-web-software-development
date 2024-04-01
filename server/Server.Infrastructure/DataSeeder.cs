using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Server.Application.Common.Dtos;
using Server.Application.Common.Extensions;
using Server.Application.Common.Interfaces.Persistence;
using Server.Domain.Common.Constants;
using Server.Domain.Entity.Content;
using Server.Domain.Entity.Identity;
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

    public static async Task SeedContribution(AppDbContext context, RoleManager<AppRole> roleManager, IContributionRepository contributionRepository)
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
        var adminId = Guid.NewGuid();
        var studentRoleId = Guid.NewGuid();
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

        if (!context.Roles.Any())
        {
            await context.Roles.AddAsync(role);
            await context.Roles.AddAsync(studentRole);
            await context.SaveChangesAsync();
        }
        var studentList = new List<AppUser>
        {
            new()
            {
                Id = Guid.NewGuid(), FirstName = "An", LastName = "Minh", Email = "student@gmail.com",
                NormalizedEmail = "student@gmail.com".ToUpperInvariant(), UserName = "student",
                NormalizedUserName = "student".ToUpperInvariant(), IsActive = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                DateCreated = DateTime.Now,
                FacultyId = facultiesList[0].Id,
                Avatar = "/default.png"

            },
            new()
            {
                Id = Guid.NewGuid(), FirstName = "Vu", LastName = "Nguyen", Email = "student1@gmail.com",
                NormalizedEmail = "student1@gmail.com".ToUpperInvariant(), UserName = "student1",
                NormalizedUserName = "student1".ToUpperInvariant(), IsActive = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                DateCreated = DateTime.Now,
                FacultyId = facultiesList[1].Id,
                Avatar = "/default.png"

            },
            new()
            {
                Id = Guid.NewGuid(), FirstName = "Hung", LastName = "Tran", Email = "trankyhung225@gmail.com",
                NormalizedEmail = "trankyhung225@gmail.com".ToUpperInvariant(), UserName = "student2",
                NormalizedUserName = "student2".ToUpperInvariant(), IsActive = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                DateCreated = DateTime.Now,
                FacultyId = facultiesList[2].Id,
                Avatar = "/default.png"

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
                Avatar = "/default.png"

            },
        };
        foreach (var user in studentList)
        {
            user.PasswordHash = passwordHasher.HashPassword(user, "Admin123@");
        }
        if (!context.Users.Any())
        {
            // admin
            
            var userEmail = "admin@gmail.com";
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
                Avatar = "/default.png",
                FacultyId = facultiesList[1].Id,
            };
            user.PasswordHash = passwordHasher.HashPassword(user, "Admin123@");

            await context.Users.AddAsync(user);

            await context.UserRoles.AddAsync(new IdentityUserRole<Guid>
            {
                RoleId = rootAdminRoleId,
                UserId = adminId
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
                        Value = "Permissions.Contributions.Create"
                    },
                    new()
                    {
                        Selected = true,
                        Value = "Permissions.Contributions.Edit"
                    },
                };
                foreach (var permission in studentPermissionList)
                {
                    await roleManager.AddClaimAsync(studentRole, new Claim("permissions", permission.Value!));
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
            },
            new()
            {
                AcademicYearId = yearList[1].Id,
                FacultyId = facultiesList[1].Id,
                UserId = studentList[1].Id,
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
                UserId =studentList[2].Id,
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
                UserId = studentList[3].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 4",
                Slug = "test-4",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
            },
            new()
            {
                AcademicYearId = yearList[0].Id,
                FacultyId = facultiesList[0].Id,
                UserId = studentList[0].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 5",
                Slug = "test-5",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
            },
            new()
            {
                AcademicYearId = yearList[1].Id,
                FacultyId = facultiesList[1].Id,
                UserId = studentList[1].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 6",
                Slug = "test-6",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
            },
            new()
            {
                AcademicYearId = yearList[0].Id,
                FacultyId = facultiesList[2].Id,
                UserId =studentList[2].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 7",
                Slug = "test-7",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
            },
            new()
            {
                AcademicYearId = yearList[1].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[3].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 8",
                Slug = "test-8",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
            },
             new()
            {
                AcademicYearId = yearList[0].Id,
                FacultyId = facultiesList[0].Id,
                UserId = studentList[0].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 9",
                Slug = "test-9",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
            },
            new()
            {
                AcademicYearId = yearList[1].Id,
                FacultyId = facultiesList[1].Id,
                UserId = studentList[1].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 10",
                Slug = "test-10",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
            },
            new()
            {
                AcademicYearId = yearList[0].Id,
                FacultyId = facultiesList[2].Id,
                UserId =studentList[2].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 11",
                Slug = "test-11",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
            },
            new()
            {
                AcademicYearId = yearList[1].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[3].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 12",
                Slug = "test-12",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
            },
            new()
            {
                AcademicYearId = yearList[0].Id,
                FacultyId = facultiesList[0].Id,
                UserId = studentList[0].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 13",
                Slug = "test-13",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
            },
            new()
            {
                AcademicYearId = yearList[1].Id,
                FacultyId = facultiesList[1].Id,
                UserId = studentList[1].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 14",
                Slug = "test-14",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
            },
            new()
            {
                AcademicYearId = yearList[0].Id,
                FacultyId = facultiesList[2].Id,
                UserId =studentList[2].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 15",
                Slug = "test-15",
                SubmissionDate = DateTime.Now,
                Status = ContributionStatus.Pending,
            },
            new()
            {
                AcademicYearId = yearList[1].Id,
                FacultyId = facultiesList[3].Id,
                UserId = studentList[3].Id,
                Id = Guid.NewGuid(),
                IsConfirmed = true,
                DateCreated = DateTime.Now,
                Title = "test 16",
                Slug = "test-16",
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
        // add file 
        if (!context.Files.Any())
        {
            foreach (var contribution in listContribution)
            {
                await context.Files.AddRangeAsync(new List<File>
                {
                    new()
                    {
                        ContributionId = contribution.Id, DateCreated = DateTime.UtcNow, Id = Guid.NewGuid(),
                        Path = "/default.png", Name = "default.png", Type = FileType.Thumbnail
                    },
                    new()
                    {
                        ContributionId = contribution.Id, DateCreated = DateTime.UtcNow, Id = Guid.NewGuid(),
                        Path = "/default.png", Name = "default.png", Type = FileType.File
                    },
                });
            }
        }

        // approve
        if (!context.ContributionPublics.Any())
        {
            await contributionRepository.Approve(listContribution[0], adminId);
            await contributionRepository.Approve(listContribution[1], adminId); 
            await contributionRepository.Approve(listContribution[2], adminId);
            await contributionRepository.Approve(listContribution[3], adminId);
            await contributionRepository.Approve(listContribution[4], adminId);
            await contributionRepository.Approve(listContribution[5], adminId);
            await contributionRepository.Approve(listContribution[6], adminId);
            await contributionRepository.Approve(listContribution[7], adminId);

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