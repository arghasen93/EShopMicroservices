using BuildingBlocks.Behaviors;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter(new DependencyContextAssemblyCatalog([typeof(Program).Assembly]));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddCortexMediator(builder.Configuration, [typeof(Program)], 
    config => {
        config.OnlyPublicClasses = false;
        config.AddOpenCommandPipelineBehavior(typeof(ValidationCommandBehavior<,>));
        config.AddOpenQueryPipelineBehavior(typeof(ValidationQueryBehavior<,>));
        config.AddDefaultBehaviors();
    }
);
builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("ProductCatalogDatabase")!);
}).UseLightweightSessions();


var app = builder.Build();

app.MapCarter();

app.Run();
