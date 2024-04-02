using System.Net;
using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Server.Application.Common.Interfaces.Authentication;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Common.Interfaces.Services;
using Server.Domain.Entity.Identity;
using Server.Infrastructure.Authentication;
using Server.Infrastructure.Persistence;
using Server.Infrastructure.Persistence.Repositories;
using Server.Infrastructure.Services;
using Server.Infrastructure.Services.Email;
using Server.Infrastructure.Services.Media;

namespace Server.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<,>), typeof(RepositoryBase<,>));
        services.AddRepositories();
        // email settings
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.AddTransient<IEmailService, EmailService>();
        // upload file
        services.Configure<MediaSettings>(configuration.GetSection("MediaSettings"));
        // cloudinary
        services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));

        services.AddTransient<IMediaService, MediaService>();
        services
            .AddDatabase(configuration)
            .AddDbIdentity()
            .AddAuth(configuration);

        // AddAuthentication must below AddIdentity because it will redirect to new page by Identity

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        var concreteServices = typeof(FalcutyRepository).Assembly.GetTypes()
            .Where(x => x.GetInterfaces().Any(i => i.Name == typeof(IRepository<,>).Name)
                && !x.IsAbstract
                && x.IsClass
                && !x.IsGenericType);

        foreach (var concreteService in concreteServices)
        {
            var allInterfaces = concreteService.GetInterfaces();

            var directInterface =
                allInterfaces
                .Except(allInterfaces
                    .SelectMany(t => t.GetInterfaces()))
                .FirstOrDefault();

            if (directInterface != null)
            {
                services.Add(new ServiceDescriptor(directInterface, concreteService, ServiceLifetime.Scoped));
            }
        }        

        return services;
    }

    public static IServiceCollection AddAuth(this IServiceCollection services,
                                             ConfigurationManager configuration)
    {
        services.AddSingleton<ITokenService, TokenService>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);
        services.AddSingleton(Options.Create(jwtSettings));

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(options =>
           {
               options.IncludeErrorDetails = true;
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateLifetime = false,
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = jwtSettings.Issuer,
                   ValidAudience = jwtSettings.Audience,
                   IssuerSigningKey = new SymmetricSecurityKey(
                       Encoding.UTF8.GetBytes(jwtSettings.Secret)
                   ),
               };

               options.Events = new JwtBearerEvents
               {
                   OnAuthenticationFailed = authenticationFailedContext =>
                   {
                       string result = string.Empty;

                       authenticationFailedContext.Response.ContentType = MediaTypeNames.Application.Json;

                       if (authenticationFailedContext.Exception is SecurityTokenExpiredException)
                       {
                           authenticationFailedContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                           result = JsonConvert.SerializeObject(new { message = "Token Invalid" });
                       }
                       else
                       {
                           authenticationFailedContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                           result = JsonConvert.SerializeObject(new { message = "An unhandled error has occured." });
                       }

                       return authenticationFailedContext.Response.WriteAsync(result);
                   },

                   OnChallenge = jwtBearerChallengeContext =>
                   {
                       jwtBearerChallengeContext.HandleResponse();

                       if (!jwtBearerChallengeContext.Response.HasStarted)
                       {
                           jwtBearerChallengeContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                           jwtBearerChallengeContext.Response.ContentType = MediaTypeNames.Application.Json;
                           var result = JsonConvert.SerializeObject(new { message = "You are not authorized." });
                           return jwtBearerChallengeContext.Response.WriteAsync(result);
                       }

                       return Task.CompletedTask;
                   },

                   OnForbidden = forbiddenContext =>
                   {
                       forbiddenContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                       forbiddenContext.Response.ContentType = MediaTypeNames.Application.Json;
                       var result = JsonConvert.SerializeObject(new { message = "You are not authorized to access these resources" });
                       return forbiddenContext.Response.WriteAsync(result);
                   }
               };
           });

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }

    public static IServiceCollection AddDbIdentity(this IServiceCollection services)
    {
        services.AddIdentity<AppUser, AppRole>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            // Password settings.
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings.
            options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = false;
        });
        // forgot password token lifetime
        services.Configure<DataProtectionTokenProviderOptions>(opt =>
            opt.TokenLifespan = TimeSpan.FromSeconds(30));
        return services;
    }


}