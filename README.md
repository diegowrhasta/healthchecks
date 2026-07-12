# HealthChecks Project

Project built with .NET Aspire to test out a prototype that leverages Prometheus and Grafana to build a neat HealthChecks 
dashboard to which the health of different services can be looked at a quick glance.

## Docker on Aspire

Apparently, just so you can build with Aspire, you need a `docker-buildx` plugin installed, otherwise Aspire won't be able 
to start the docker build process as he wants to.

But this enables you to then always on aspire start, build images that you then spin up:

````csharp
var apiService = builder
    .AddDockerfile(name: "apiservice", contextPath: "../", dockerfilePath: "./HealthChecks.ApiService/Dockerfile")
    .WithHttpEndpoint(targetPort: 8080, port: 5591)
    .WithHttpHealthCheck("/health");
````

Now, this comes with the caveat that it takes a long time to start. But for this specific prototype it makes sense because 
we really want to emulate a whole architecture that's already interconnected, and to get across the hurdle of in-process apps 
that can't be reliably connected from container's we can just make them containers themselves.

## Healthcheck dashboard

Found a better, sleek, and clean way of making the dashboard. (Better than healthchecks.io). It leverages the native functionalities 
of Prometheus and then its connection to Grafana.

Because we can define in prometheus different jobs to scrape information, when querying inside of it, we can use literal 
syntax to get info on all these scraping jobs. And with that in mind, if a job goes down in scraping, we can arguably determine 
that the service is down, or has issues, because a simple scrape of its metrics endpoint should be healthy and working if the 
service is healthy and working.

This even takes away the need of having to code ourselves some sorth of health, a literal scraping would be more than enough.