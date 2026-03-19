using Ordering.Application.Extensions;

namespace Ordering.Application.Orders.Commands.UpdateOrder;

public class UpdateOrderHandler(IApplicationDbContext dbContext): ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
{
    public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        var orderId = OrderId.Of(command.Order.Id);
        var order = await dbContext.Orders.FindAsync([orderId], cancellationToken);
        if (order is null)
        {
            throw new OrderNotFoundException(nameof(Order), command.Order.Id);
        }

        UpdateOrderWithNewValues(order, command.Order);
        dbContext.Orders.Update(order);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateOrderResult(true);
    }

    private static void UpdateOrderWithNewValues(Order order, OrderDto orderDto)
    {
        order.Update(
            orderName: OrderName.Of(orderDto.OrderName),
            shippingAddress: orderDto.ShippingAddress.ToAddress(),
            billingAddress: orderDto.BillingAddress.ToAddress(),
            payment: orderDto.Payment.ToPayment(),
            status: orderDto.Status);
    }
}