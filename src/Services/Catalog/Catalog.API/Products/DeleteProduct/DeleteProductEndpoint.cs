namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductRequest(Guid Id);
public record DeleteProductResponse(bool IsSuccess);
public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{id:guid}", async (Guid Id, IMediator sender) =>
        {
            var command = new DeleteProductCommand(Id);
            var result = await sender.SendCommandAsync<DeleteProductCommand, DeleteProductResult>(command);
            var response = result.Adapt<DeleteProductResponse>();
            return Results.Ok(response);
        })
        .WithTags("DeleteProduct")
        .WithDescription("Deletes an existing product.")
        .WithSummary("Delete Product")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
