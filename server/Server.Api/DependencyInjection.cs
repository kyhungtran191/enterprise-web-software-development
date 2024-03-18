using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Server.Api.Common.Errors;
using Server.Domain.Entity.Identity;
using Server.Infrastructure;

namespace Server.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();              
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddSingleton<ProblemDetailsFactory, ServerProblemDetailsFactory>();

        return services;
    }
}

public static class MigrationManager
{
    public static WebApplication MigrateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        using var anBlogContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

        // applying update-database command
        anBlogContext.Database.Migrate();
        DataSeeder.SeedAsync(anBlogContext, roleManager).GetAwaiter().GetResult();

        return app;
    }
}