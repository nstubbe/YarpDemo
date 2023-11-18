# YARP Demo projects collection

This is a collection of YARP demo projects. Each project is a self-contained example of how to use YARP in a specific
context.

### What is YARP?

YARP (Yet Another Reverse Proxy) is a reverse proxy that is built on top of ASP.NET Core. This means you can use all the
features of ASP.NET Core and C# to write your own custom proxy logic. YARP is also a middleware, which means you can use
it in any ASP.NET Core application.

More on YARP can be found here: https://microsoft.github.io/reverse-proxy/

### Contents of this repo

Here's a short summary of each of the projects in this repo and the features they demonstrate:

* DemoYARP.ReverseProxy (WIP) - A basic reverse proxy that shows a very simple configuration and how to use the YARP
  middleware.
* DemoYARP.LoadBalancer (WIP) - Gives an example of how to use the YARP middleware to implement a load balancer.
* DemoYARP.HeaderRouter (WIP) - YARP has a bunch of built-in routing policies. This project shows how to use the header
  routing policy
  with a tenant header to route to different backends.
* DemoYARP.Transforms (WIP) - This project shows you how to rewrite your requests and responses using the built-in transforms.
* DemoYARP.BlueGreen - A (very basic) example of a blue-green deployment setup using YARP. This project also shows you
  how you can
  change the
  configuration of YARP at runtime, and exposes an API endpoint in the proxy to switch between the blue and green
  deployments. This is the most advanced project in this repo.

### Running the projects

The projects dont have any dependencies on each other, so you can run them independently. Keep in mind however that to
demonstrate some capabilities, you want to be able to proxy to one or more websites. That's why there are also two
projects
that are used to test the YARP configuration. They are:

* DemoYARP.site
* DemoYarp.sitetwo

These are just two Blazor WASM sites so the YARP projects have something to proxy to. They do not contain any special
logic or features.




