using Logging;
using NLog;
using NLog.Web;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();



var builder = WebApplication
    .CreateBuilder(args)
    ;

builder.Host.UseNLog();

builder
    .CreateStartup()
    .Build()
    .ConfigureStartup(builder)
    .Run();
