using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Server.Api.Authorization;
using Server.Api.Common.Errors;
using Server.Api.Common.Filters;
using Server.Application.Common.Interfaces.Persistence;
using Server.Domain.Entity.Identity;
using Server.Infrastructure;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Server.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.CustomOperationIds(apiDesc =>
            {
                return apiDesc.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null;
            });

            c.SwaggerDoc("AdminAPI", new OpenApiInfo
            {
                Version = "v1",
                Title = "API for Administrators",
                Description = "API for CMS core domain. This domain keeps track of campaigns,  campaign rules, and campagin execution."
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "Input your Bearer Token in this format - Bearer {your token here} to access this API"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "Bearer",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });

            c.ParameterFilter<SwaggerNullableParameterFilter>();
        });
        
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        services.AddSingleton<ProblemDetailsFactory, ServerProblemDetailsFactory>();

        services.AddAuthorization();

        return services;
    }

    public static IServiceCollection AddCors(this IServiceCollection services,
                                             ConfigurationManager configuration,
                                             string serverCorsPolicy)
    {
        services.AddCors(o => o.AddPolicy(serverCorsPolicy, builderCors =>
        {
            var origins = configuration["AllowedOrigins"];

            builderCors
            .AllowAnyMethod()
            .WithOrigins(origins!)
            .AllowAnyHeader()
            .AllowCredentials();
        }));

        return services;
    }

    public static IServiceCollection AddAuthorization(this IServiceCollection services)
    {

        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

        return services;
    }
}

public static class MigrationManager
{
    public static WebApplication MigrateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        using var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
        var contributionRepository = scope.ServiceProvider.GetRequiredService<IContributionRepository>();
        //var facultyRepository = scope.ServiceProvider.GetRequiredService<IFacultyRepository>();
        //var academicYearRepository = scope.ServiceProvider.GetService<IAcademicYearRepository>();
        // applying update-database command
        appDbContext.Database.Migrate();
        // DataSeeder.SeedAsync(appDbContext, roleManager).GetAwaiter().GetResult();
        DataSeeder.SeedContribution(appDbContext,roleManager,contributionRepository).GetAwaiter().GetResult();
        //DataSeeder.SeedFaculty(appDbContext, facultyRepository).GetAwaiter().GetResult();
        //DataSeeder.SeedAcademicYear(appDbContext, academicYearRepository).GetAwaiter().GetResult();
        return app;
    }
}