namespace Catalog.API.Products.GetProductByCategory;

public record GetProductByCategoryRequest(string Category);
public record GetProductByCategoryResponse(IEnumerable<Product> Products);
public class GetProductByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category/{category}", async (string category, IMediator sender, CancellationToken cancellationToken) =>
        {
            var query = new GetProductByCategoryQuery(category);
            var result = await sender.SendQueryAsync<GetProductByCategoryQuery, GetProductByCategoryResult>(query, cancellationToken);
            var response = result.Adapt<GetProductByCategoryResponse>();
            return Results.Ok(response);
        })
        .WithDescription("Get Products by Category")
        .WithTags("Product")
        .Produces<Product>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
