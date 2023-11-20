using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Configuration;

namespace DemoYARP.bluegreen.ProxyConfig;

/// <summary>
///     Using a custom config provider allows us to change the config at runtime.
/// </summary>
public class CustomConfig : IProxyConfig
{
    private readonly CancellationTokenSource _cts = new();

    public CustomConfig(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
    {
        Routes = routes;
        Clusters = clusters;
        ChangeToken = new CancellationChangeToken(_cts.Token);
    }

    public IReadOnlyList<RouteConfig> Routes { get; }

    public IReadOnlyList<ClusterConfig> Clusters { get; }

    public IChangeToken ChangeToken { get; }

    internal void SignalChange()
    {
        _cts.Cancel();
    }
}