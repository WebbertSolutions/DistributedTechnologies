using DistributedPayment;

var builder = WebApplication
    .CreateBuilder(args)
    ;

builder
    .CreateStartup()
    .Build()
    .ConfigureStartup(builder)
    .Run();
