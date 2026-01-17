namespace Catalog.API.Products.CreateProduct;

public record CreateProductRequest(string Name, string Description, string ImageFile, decimal Price, List<string> Categories);
public record CreateProductResponse(Guid Id);
public class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (CreateProductRequest request, IMediator sender, CancellationToken cancellationToken) =>
        {
            var command = request.Adapt<CreateProductCommand>();
            var result = await sender.SendCommandAsync<CreateProductCommand, CreateProductResult>(command, cancellationToken);
            var response = result.Adapt<CreateProductResponse>();
            return Results.Created($"/products/{result.Id}", response);
        })
        .WithTags("CreateProduct")
        .WithDescription("Creates a new product.")
        .WithSummary("Create Product")
        .Produces<CreateProductResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
