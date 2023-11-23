var builder = WebApplication.CreateBuilder(args);

// The easiest way to configure YARP is to get the config from your appsettings.
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
var app = builder.Build();
app.MapReverseProxy();
app.Run();