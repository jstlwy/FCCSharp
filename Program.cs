using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
using FCCSharp.Data;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<ExerciseDbContext>(options =>
        {
            options.UseSqlite(builder.Configuration.GetConnectionString("ExerciseDbContext") ?? throw new InvalidOperationException("Connection string 'ExerciseDbContext' not found."));
        });
        builder.Services.AddDbContext<ShortUrlDbContext>(options =>
        {
            options.UseSqlite(builder.Configuration.GetConnectionString("ShortUrlDbContext") ?? throw new InvalidOperationException("Connection string 'ShortUrlDbContext' not found."));
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        // Set up routes and controllers
        app.MapGet("/whoami", (HttpContext context, HttpRequest request) =>
        {
            ConnectionInfo ci = context.Connection;
            string clientIP = (ci.RemoteIpAddress != null) ? ci.RemoteIpAddress.ToString() : "unknown";

            return Results.Ok(new
            {
                ipaddress = clientIP,
                language = request.Headers.AcceptLanguage.ToString(),
                software = request.Headers.UserAgent.ToString()
            });
        });
        app.MapGet("/hello", () =>
        {
            return Results.Ok(new
            {
                greeting = "Hello, world!"
            });
        });
        app.MapGet("/date", () =>
        {
            DateTimeOffset now = DateTimeOffset.Now;
            return Results.Ok(new
            {
                unixdate = now.ToUnixTimeSeconds(),
                utcdate = now
            });
        });
        app.MapControllers();

        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.Run();
    }
}