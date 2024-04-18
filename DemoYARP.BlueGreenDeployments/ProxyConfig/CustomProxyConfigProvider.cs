using Yarp.ReverseProxy.Configuration;

namespace DemoYARP.bluegreen.ProxyConfig;

public class CustomProxyConfigProvider : IProxyConfigProvider
{
    private const string ServerOneAddress = "http//127.0.0.1:8080";
    private const string ServerTwoAddress = "http//127.0.0.1:8081";
    
    private const string ClusterOne = "ClusterOne";
    private const string RouteOne = "RouteOne";
    
    private CustomConfig _config;

    private bool UseSecondAddress = false;
    
    public CustomProxyConfigProvider()
    {
        var routes = GenerateRoutes();
        var clusters = GenerateClusters();
        _config = new CustomConfig(routes, clusters);
    }
    
    public IProxyConfig GetConfig()
    {
        return _config;
    }
    
    private List<RouteConfig> GenerateRoutes()
    {
        // Example of generating routes
        var routes = new List<RouteConfig>
        {
            new RouteConfig
            {
                RouteId = RouteOne,
                ClusterId = ClusterOne,
                Match = new RouteMatch { Path = "{**catch-all}" }
            }
        };
        return routes;
    }

    private List<ClusterConfig> GenerateClusters()
    {
        var clusters = new List<ClusterConfig>
        {
            new ClusterConfig
            {
                ClusterId = ClusterOne,
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    { "destination1", new DestinationConfig { Address = UseSecondAddress ? ServerTwoAddress : ServerOneAddress} }
                }
            }
        };
        return clusters;
    }

    public void SwitchDestination()
    {
        UseSecondAddress = !UseSecondAddress;
        
        var routes = GenerateRoutes();
        var clusters = GenerateClusters();
        
        _config = new CustomConfig(routes, clusters);
        _config.SignalChange();
    }
}