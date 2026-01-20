namespace Basket.api.Exceptions;

public class BasketNotFoundException(string userName) : NotFoundException("Basket", userName) {}
