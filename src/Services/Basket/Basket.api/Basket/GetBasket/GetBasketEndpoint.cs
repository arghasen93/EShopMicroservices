namespace Basket.api.Basket.GetBasket;

public record GetBasketRequest(string UserName);
public record GetBasketResponse(ShoppingCart Cart);
public class GetBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{userName}", async (string userName, IMediator sender, CancellationToken cancellationToken) =>
        {
            var query = new GetBasketQuery(userName);
            var result = await sender.SendQueryAsync<GetBasketQuery, GetBasketResult>(query, cancellationToken);
            var response = result.Adapt<GetBasketResponse>();
            return Results.Ok(response);
        })
        .WithDescription("Get Basket")
        .WithName("Basket")
        .Produces<GetBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
