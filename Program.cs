using JsonPlaceholderProxyApi.Services;
using JsonPlaceholderProxyApi.Services.Interfaces;
using JsonPlaceholderProxyApi.Middleware;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// HttpClient for calling JSONPlaceholder APIs
builder.Services.AddHttpClient();

// REQUIRED for CorrelationId access in services
builder.Services.AddHttpContextAccessor();

// Dependency Injection
builder.Services.AddScoped<IPostService, PostService>();

var app = builder.Build();


//
// --------------------
// Runtime info (Docker check)
// --------------------
var isRunningInDocker =
    Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");

Log.Information(
    "Application starting | Environment={Environment} | RunningInDocker={Docker}",
    app.Environment.EnvironmentName,
    isRunningInDocker ?? "false");



// Serilog initialization (SAFE LOCATION)
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Fatal)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .WriteTo.Console()
    .WriteTo.File(
        path: "Logs/app-.log",
        rollingInterval: RollingInterval.Day,
        shared: true)
    .CreateLogger();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Correlation ID middleware
app.UseMiddleware<CorrelationIdMiddleware>();

// Authorization middleware
app.UseAuthorization();
app.MapControllers();


// --------------------
// Optional: env check endpoint (safe to keep)
// --------------------
app.MapGet("/env", () => new
{
    Environment = app.Environment.EnvironmentName,
    IsRunningInDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"),
    MachineName = Environment.MachineName
});
app.Run();
