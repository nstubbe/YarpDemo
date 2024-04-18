using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Add the rate limiter middleware to your services
// We are using the fixed window rate limiter here and are naming our policy "fixedRateLimiter"
builder.Services.AddRateLimiter(_ => _
    .AddFixedWindowLimiter(policyName: "fixedRateLimiter", options =>
    {
        options.PermitLimit = 10;
        options.Window = TimeSpan.FromSeconds(12);
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 0;
    })
);

var app = builder.Build();

// Don't forget to use the rate limiter middleware!
app.UseRateLimiter();
app.MapReverseProxy();

app.Run();