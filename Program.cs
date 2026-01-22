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

// Serilog initialization (SAFE LOCATION)
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
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

app.Run();
