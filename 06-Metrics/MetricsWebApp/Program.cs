// https://docs.microsoft.com/en-us/samples/azure-samples/application-insights-aspnet-sample-opentelemetry/update-this-to-unique-url-stub/
// https://docs.microsoft.com/en-us/azure/azure-monitor/app/opentelemetry-enable?tabs=net

using MetricsWebApp;

var builder = WebApplication
    .CreateBuilder(args)
    ;

builder
    .CreateStartup()
    .Build()
    .ConfigureStartup(builder)
    .Run();
