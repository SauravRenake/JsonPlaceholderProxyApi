using JsonPlaceholderProxyApi.Services;
using JsonPlaceholderProxyApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// HttpClient for calling JSONPlaceholder APIs
builder.Services.AddHttpClient();

// Dependency Injection
builder.Services.AddScoped<IPostService, PostService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// IMPORTANT: Maps controller routes (e.g. /posts)
app.MapControllers();

app.Run();
