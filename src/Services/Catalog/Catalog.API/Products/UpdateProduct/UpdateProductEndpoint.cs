namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductRequest(string Name, List<string> Categories, string Description, string ImageFile, decimal Price);

public record UpdateProductResponse(bool IsSuccess);
public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products/{id:guid}", async (Guid Id, UpdateProductRequest request, IMediator sender, CancellationToken cancellationToken) =>
        {
            var command = request.Adapt<UpdateProductCommand>();
            command = command with { Id = Id };
            var result = await sender.SendCommandAsync<UpdateProductCommand, UpdateProductResult>(command, cancellationToken);
            var response = result.Adapt<UpdateProductResponse>();
            return Results.Ok(response);
        })
        .WithTags("UpdateProduct")
        .WithDescription("Updates an existing product.")
        .WithSummary("Update Product")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
