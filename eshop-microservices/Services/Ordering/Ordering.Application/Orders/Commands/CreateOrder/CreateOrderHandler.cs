
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
        var shippingAddress = Address.Of(orderDto.ShippingAddress.FirstName, orderDto.ShippingAddress.LastName, orderDto.ShippingAddress.EmailAddress, orderDto.ShippingAddress.AddressLine, orderDto.ShippingAddress.Country, orderDto.ShippingAddress.State, orderDto.ShippingAddress.ZipCode);
        var billingAddress = Address.Of(orderDto.BillingAddress.FirstName, orderDto.BillingAddress.LastName, orderDto.BillingAddress.EmailAddress, orderDto.BillingAddress.AddressLine, orderDto.BillingAddress.Country, orderDto.BillingAddress.State, orderDto.BillingAddress.ZipCode);
        var payment  = Payment.Of(orderDto.Payment.CardNumber, orderDto.Payment.CardName, orderDto.Payment.ExpirationDate, orderDto.Payment.Cvv, orderDto.Payment.PaymentMethod);
        var newOrder = Order.Create(id: OrderId.Of(Guid.NewGuid()), customerId: CustomerId.Of(orderDto.CustomerId),
            orderName: OrderName.Of(orderDto.OrderName), shippingAddress: shippingAddress,
            billingAddress: billingAddress, payment: payment);
        
        foreach(var orderItemDto in orderDto.OrderItems)
        {
            newOrder.Add(ProductId.Of(orderItemDto.ProductId), orderItemDto.Quantity, orderItemDto.Price);
        }
        return newOrder;
    }
    

    
}