using Microsoft.EntityFrameworkCore;

using ScrimHub.Api;
using ScrimHub.Infrastructure;

using Serilog;

const string DisconnectedSql =
    "Server=127.0.0.1,1;Database=ScrimHub;User Id=sa;Password=unused;TrustServerCertificate=True;Connect Timeout=1;Encrypt=False";

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    var sqlConnection = LocalEnv.TryReadSqlConnection(builder.Environment.ContentRootPath);
    if (!string.IsNullOrWhiteSpace(sqlConnection))
    {
        builder.Configuration["ConnectionStrings:Sql"] = sqlConnection;
    }
}

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration).WriteTo.Console());

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();

var sql = builder.Configuration.GetConnectionString("Sql");
if (string.IsNullOrWhiteSpace(sql))
{
    sql = DisconnectedSql;
}

builder.Services.AddDbContext<ScrimHubDbContext>(options =>
    options.UseSqlServer(
        sql,
        sqlOptions => sqlOptions.MigrationsAssembly(typeof(ScrimHubDbContext).Assembly.GetName().Name)));

var app = builder.Build();

app.UseExceptionHandler();
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/health", () => Results.Ok(new HealthResponse("ok", "api")))
    .WithName("GetHealth");

app.MapGet("/health/ready", async (ScrimHubDbContext db, CancellationToken cancellationToken) =>
{
    try
    {
        if (await db.Database.CanConnectAsync(cancellationToken))
        {
            return Results.Ok(new HealthResponse("ok", "api"));
        }
    }
    catch (Exception ex)
    {
        Log.Warning("SQL readiness check failed: {ExceptionType}", ex.GetType().Name);
    }

    return Results.Json(
        new HealthResponse("unavailable", "api"),
        statusCode: StatusCodes.Status503ServiceUnavailable);
}).WithName("GetHealthReady");

app.Run();

public partial class Program;