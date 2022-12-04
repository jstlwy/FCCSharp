var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
	string clientIP;
	if (ci.RemoteIpAddress != null)
		clientIP = ci.RemoteIpAddress.ToString();
	else
		clientIP = "unknown";
	return Results.Ok(new {
		ipaddress = clientIP,
		language = request.Headers.AcceptLanguage.ToString(),
		software = request.Headers.UserAgent.ToString()
	});
});
app.MapGet("/hello", () =>
{
	return Results.Ok(new {
		greeting = "Hello, world!"
	});
});
app.MapGet("/date", () =>
{
	DateTimeOffset now = DateTimeOffset.Now;
	return Results.Ok(new {
		unixdate = now.ToUnixTimeSeconds(),
		utcdate = now
	});
});
app.MapControllers();

app.Run();
