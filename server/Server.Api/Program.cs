using Server.Api;
using Server.Application;
using Server.Infrastructure;

// http://localhost:5272/swagger/index.html

var builder = WebApplication.CreateBuilder(args);

var serverCorsPolicy = "ServerCorsPolicy";

// Add services to the container.
{
    builder.Services
        .AddCors(builder.Configuration, serverCorsPolicy)
        .AddPresentation()
        .AddApplication()
        .AddInfrastructure(builder.Configuration);
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapGet("/debug/routes", (IEnumerable<EndpointDataSource> endpointSources) =>
           string.Join("\n", endpointSources.SelectMany(source => source.Endpoints)));

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("AdminAPI/swagger.json", "Admin API");
        c.DisplayOperationId();
        c.DisplayRequestDuration();
    });

    app.MigrateDatabase();
}

{
    app.UseExceptionHandler("/error");

    app.UseCors(serverCorsPolicy);

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}


