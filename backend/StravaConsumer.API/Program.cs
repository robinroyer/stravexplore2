using StravaConsumer.API.Services;
using Microsoft.Extensions.Caching.Memory;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Memory Cache
builder.Services.AddMemoryCache();

// HTTP Client with Polly retry policy
var retryPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

builder.Services.AddHttpClient<IStravaApiService, StravaApiService>()
    .AddPolicyHandler(retryPolicy);

// Register services
builder.Services.AddScoped<IStravaApiService, StravaApiService>();
builder.Services.AddScoped<ISegmentService, SegmentService>();
builder.Services.AddScoped<IRouteService, RouteService>();
builder.Services.AddScoped<ITrendService, TrendService>();
builder.Services.AddScoped<ICacheService, CacheService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();
