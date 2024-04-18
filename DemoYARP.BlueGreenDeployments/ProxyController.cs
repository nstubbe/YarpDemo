using DemoYARP.bluegreen.ProxyConfig;
using Microsoft.AspNetCore.Mvc;
using Yarp.ReverseProxy.Configuration;

namespace DemoYARP.BlueGreenDeployments;

[ApiController]
[Route("[controller]")]
public class ProxyController : ControllerBase
{
    private readonly CustomProxyConfigProvider _proxyConfigProvider;

    public ProxyController(IProxyConfigProvider proxyConfigProvider)
    {
        _proxyConfigProvider = (CustomProxyConfigProvider)proxyConfigProvider;
    }

    [HttpGet]
    public async Task<IActionResult> Switch()
    {
        var newClusterId = _proxyConfigProvider.SwitchCluster();
        return Ok(new { newClusterId });
    }
}