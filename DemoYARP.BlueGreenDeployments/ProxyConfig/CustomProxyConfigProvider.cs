using Yarp.ReverseProxy.Configuration;

namespace DemoYARP.bluegreen.ProxyConfig;

/// <summary>
///     This class is used to create a custom config provider for the YARP reverse proxy.
///     You will notice that it contains a lot of configuration that is normally read from appsettings.json.
///     We are loading the config from code instead of from appsettings.json to allow us to change the config at runtime.
///     You could also load the config from another data source, such as a database or a file.
/// </summary>
public class CustomProxyConfigProvider : IProxyConfigProvider
{
    private const string BlueClusterId = "BlueCluster";
    private const string GreenClusterId = "GreenCluster";
    private const string AppRouteId = "AppRoute";

    private const string BlueAddress = "http://127.0.0.1:8080";
    private const string GreenAddress = "http://127.0.0.1:8081";

    /// The config for the app clusters. There are two: a blue and a green cluster.
    /// Only one will be active at a time.
    private readonly ClusterConfig[] _appClusterConfigs =
    {
        new()
        {
            ClusterId = BlueClusterId,
            Destinations = new Dictionary<string, DestinationConfig>
            {
                { "blue", new DestinationConfig { Address = BlueAddress } }
            }
        },
        new()
        {
            ClusterId = GreenClusterId,
            Destinations = new Dictionary<string, DestinationConfig>
            {
                { "green", new DestinationConfig { Address = GreenAddress } }
            }
        }
    };

    /// The config for the app route will match everything that is not found in the controllers of this API.
    private readonly RouteConfig _appRouteConfig = new()
    {
        RouteId = AppRouteId,
        ClusterId = BlueClusterId,
        Match = new RouteMatch
        {
            Path = "{**catch-all}"
        }
    };

    private CustomConfig _config;

    public CustomProxyConfigProvider()
    {
        var routeConfigs = new[] { _appRouteConfig };
        _config = new CustomConfig(routeConfigs, _appClusterConfigs);
    }

    /// Called when the proxy is initialized. This method is called only once per application lifetime.
    public IProxyConfig GetConfig()
    {
        return _config;
    }

    /// Switches between the blue and green clusters by checking which one is active and switching to the other one.
    /// This is done by creating a new route config with the new cluster and calling the update method.
    public string SwitchCluster()
    {
        var activeConfig = _config;
        // Get the active app route config
        var activeAppRouteConfig = activeConfig.Routes.First(r => r.ClusterId is GreenClusterId or BlueClusterId);

        // Switch to the other cluster
        var newClusterId = activeAppRouteConfig.ClusterId == BlueClusterId ? GreenClusterId : BlueClusterId;

        // Create a new route config with the new cluster
        var newRoute = new RouteConfig
        {
            RouteId = activeAppRouteConfig.RouteId,
            ClusterId = newClusterId,
            Match = new RouteMatch
            {
                Path = activeAppRouteConfig.Match.Path
            }
        };
        var newRoutes = new[] { newRoute };
        Update(newRoutes, _config.Clusters);

        // Return the new cluster id so we can use it in the response
        return newClusterId;
    }

    /// Updates the config with the new routes and clusters and invalidates the change token.
    private void Update(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
    {
        var oldConfig = _config;
        _config = new CustomConfig(routes, clusters);
        oldConfig.SignalChange();
    }
}