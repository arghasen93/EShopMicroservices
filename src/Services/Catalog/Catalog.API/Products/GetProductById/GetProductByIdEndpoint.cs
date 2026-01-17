namespace Catalog.API.Products.GetProductById;

public record GetProductByIdRequest(Guid Id);
public record GetProductByIdResponse(Product? Product);

public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id:guid}", async (Guid id, IMediator sender, CancellationToken cancellationToken) =>
        {
            var query = new GetProductByIdQuery(id);
            var result = await sender.SendQueryAsync<GetProductByIdQuery, GetProductByIdResult>(query);
            var response = result.Adapt<GetProductByIdResponse>();
            return Results.Ok(response);
        })
        .WithName("GetProductById")
        .WithTags("Products")
        .Produces<Product>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
