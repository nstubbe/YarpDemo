using DemoYARP.bluegreen;
using DemoYARP.bluegreen.ProxyConfig;
using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add the controllers
builder.Services.AddControllers();

// Add the YARP reverseproxy. Here we get the default config from appsettings.json.
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Add Custom YARP configprovider. This will allow us to change the config at runtime.
builder.Services.AddSingleton<IProxyConfigProvider, CustomProxyConfigProvider>();

var app = builder.Build();

// Map the controllers before YARP so we can still match on the api route.
app.MapControllers();

// Map the YARP routes
app.MapReverseProxy();

app.Run();