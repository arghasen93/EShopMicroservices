namespace Catalog.API.Products.GetProducts;

public record GetProductsResponse(IEnumerable<Product> Products);
public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (IMediator sender, CancellationToken cancellationToken) =>
        {
            var query = new GetProductsQuery();
            var result = await sender.SendQueryAsync<GetProductsQuery, GetProductsResult>(query, cancellationToken);
            var response = result.Adapt<GetProductsResponse>();
            return Results.Ok(response);
        })
        .WithTags("GetProducts")
        .WithDescription("Retrieves all products.")
        .WithSummary("Get Products")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
