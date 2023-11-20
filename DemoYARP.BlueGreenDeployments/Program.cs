using DemoYARP.bluegreen.ProxyConfig;
using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add the controllers
builder.Services.AddControllers();

// Add the YARP reverseproxy.
// In this case we will load the config using the custom proxy config provider, so we just pass in null twice.
builder.Services.AddReverseProxy().LoadFromMemory(null!, null!);

// Add Custom YARP configprovider. This will allow us to change the config at runtime.
builder.Services.AddSingleton<IProxyConfigProvider, CustomProxyConfigProvider>();

var app = builder.Build();

// Map the controllers before YARP so we can still match on the api route.
app.MapControllers();

// Map the YARP routes
app.MapReverseProxy();

app.Run();