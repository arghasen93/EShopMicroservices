namespace Basket.api.Basket.StoreBasket;

public class StoreBasketEndpoint : ICarterModule
{
    public record StoreBasketRequest(ShoppingCart Cart);
    public record StoreBasketResponse(string UserName);
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket", async (StoreBasketRequest request, IMediator sender, CancellationToken cancellationToken) =>
        {
            var command = request.Adapt<StoreBasketCommand>();
            var result = await sender.SendCommandAsync<StoreBasketCommand, StoreBasketResult>(command);
            var response = result.Adapt<StoreBasketResponse>();
            return Results.Ok(response);
        })
        .WithName("StoreBasket")
        .WithTags("Basket")
        .Produces<StoreBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
