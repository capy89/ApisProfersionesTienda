using Api.Extensions;
using AspNetCoreRateLimit;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(Assembly.GetEntryAssembly());

builder.Services.ConfigureRateLimitation();

builder.Services.ConfigureApiVersioning();

// Add services to the container.

builder.Services.ConfigureCors();

builder.Services.AddAplicationServices();

builder.Services.AddControllers(options =>
 {
     options.RespectBrowserAcceptHeader = true;
     options.ReturnHttpNotAcceptable = true;
 }).AddXmlSerializerFormatters();

builder.Services.AddDbContext<TiendaContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseIpRateLimiting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var loggerFactory=services.GetRequiredService<ILoggerFactory>();
    try
    {
        var context=services.GetRequiredService<TiendaContext>();
        await context.Database.MigrateAsync();
        await TiendaContextSeed.SeedAsync(context,loggerFactory);
    }
    catch(Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "Ocurrio un error durante la migración");
    }
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
