using DistributedShoppingCart;

var builder = WebApplication
    .CreateBuilder(args)
    ;

builder
    .CreateStartup()
    .Build()
    .ConfigureStartup(builder)
    .Run();
