using Server.Api;
using Server.Application;
using Server.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
{
    builder.Services
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
    app.UseSwaggerUI();

    app.MigrateDatabase();
}

// app.UseHttpsRedirection();

// app.UseAuthorization();
{
    app.UseExceptionHandler("/error");    
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}


