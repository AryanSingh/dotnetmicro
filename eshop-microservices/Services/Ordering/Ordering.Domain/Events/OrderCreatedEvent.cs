namespace Ordering.Domain.Events;

public class OrderCreatedEvent(Order order): IDomainEvent
{
    public Order order { get; } = order;
}