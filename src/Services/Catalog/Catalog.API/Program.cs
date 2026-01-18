using Catalog.API.Data;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handlers;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Core;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter(new DependencyContextAssemblyCatalog([typeof(Program).Assembly]));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddCortexMediator(builder.Configuration, [typeof(Program)],
    config =>
    {
        config.OnlyPublicClasses = false;
        config.AddOpenCommandPipelineBehavior(typeof(ValidationCommandBehavior<,>));
        config.AddOpenCommandPipelineBehavior(typeof(LoggingCommandBehavior<,>));
        config.AddOpenQueryPipelineBehavior(typeof(ValidationQueryBehavior<,>));
        config.AddOpenQueryPipelineBehavior(typeof(LoggingQueryBehavior<,>));
    }
);
builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("ProductCatalogDatabase")!);
}).UseLightweightSessions();

if(builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CataloginitialData>();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("ProductCatalogDatabase")!, 
        healthQuery: "select 'Current TimeStamp: ' || CURRENT_TIMESTAMP;");

var app = builder.Build();

app.MapCarter();
app.UseExceptionHandler(options => { });
app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.Run();
