namespace BuildingBlocks.Messaging.Events;

public record BasketCheckoutOrderItem(Guid ProductId, int Quantity, decimal Price);
