
namespace Basket.api.Basket.DeleteBasket
{
    public class DeleteBasketEndpoint : ICarterModule
    {
        public record DeleteBasketRequest(string UserName);
        public record DeleteBasketResponse(bool IsSuccess);
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/basket/{userName}", async (string userName, IMediator sender, CancellationToken cancellationToken) =>
            {
                var command = new DeleteBasketCommand(userName);
                var result = await sender.SendCommandAsync<DeleteBasketCommand, DeleteBasketResult>(command);
                var response = result.Adapt<DeleteBasketResponse>();
                return Results.Ok(response);
            })
            .WithName("DeleteBasket")
            .WithTags("Basket")
            .WithDescription("Deletes the basket for a specific user.")
            .WithSummary("Delete Basket")
            .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
        }
    }
}
