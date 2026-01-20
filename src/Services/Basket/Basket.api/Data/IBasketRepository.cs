namespace Basket.api.Data;

public interface IBasketRepository
{
    Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken);
    Task<ShoppingCart> StoreBasket(ShoppingCart shoppingCart, CancellationToken cancellationToken);
    Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken);
}
