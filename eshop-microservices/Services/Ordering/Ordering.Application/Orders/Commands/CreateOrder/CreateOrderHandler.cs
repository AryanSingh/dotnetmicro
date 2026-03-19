using Ordering.Application.Extensions;

namespace Ordering.Application.Orders.Commands.CreateOrder;

public class CreateOrderHandler(IApplicationDbContext dbContext) : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        //create order entity from command object
        var order = CreateNewOrder(command.Order);
        //save to database
        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new CreateOrderResult(order.Id.Value);
     
    }
    
    // public static Address Of(string firstName, string lastName, string? emailAddress, string addressLine, string country, string state, string zipCode)
    // public static Payment Of(string cardNumber, string cardName, DateTime expiration, string cvv, int paymentMethod)

    private Order CreateNewOrder(OrderDto orderDto)
    {
        var newOrder = Order.Create(
            id: OrderId.Of(Guid.NewGuid()), 
            customerId: CustomerId.Of(orderDto.CustomerId),
            orderName: OrderName.Of(orderDto.OrderName), 
            shippingAddress: orderDto.ShippingAddress.ToAddress(),
            billingAddress: orderDto.BillingAddress.ToAddress(), 
            payment: orderDto.Payment.ToPayment());
        
        foreach(var orderItemDto in orderDto.OrderItems)
        {
            newOrder.Add(ProductId.Of(orderItemDto.ProductId), orderItemDto.Quantity, orderItemDto.Price);
        }
        return newOrder;
    }
}