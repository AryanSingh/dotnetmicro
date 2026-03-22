using BasketApi.Dtos;
using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace BasketApi.Basket.CheckoutBasket;

public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckoutDto): ICommand<CheckoutBasketResult>;
public record CheckoutBasketResult(bool IsSuccess);

public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(c => c.BasketCheckoutDto).NotNull().WithMessage("BasketCheckoutDto is required.");
        RuleFor(c => c.BasketCheckoutDto.UserName).NotEmpty().WithMessage("UserName is required.");
    }
}

public class CheckoutBasketCommandHandler(IBasketRepository repository, IPublishEndpoint publishEndpoint): ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    {
        //get existing basket with total price
        var basket = await repository.GetBasket(command.BasketCheckoutDto.UserName, cancellationToken);
        if(basket == null) return new CheckoutBasketResult(false);
        //set total price on basket checkout event
        var eventMessage = command.BasketCheckoutDto.Adapt<BasketCheckoutEvent>();
        eventMessage.TotalPrice = basket.TotalPrice;
        eventMessage.OrderItems = basket.Items.Select(item => new BasketCheckoutOrderItem(item.ProductId, item.Quantity, item.Price)).ToList();
        // send checkout event to masstransit
        // delete the basket
        await publishEndpoint.Publish(eventMessage, cancellationToken);
        await repository.DeleteBasket(command.BasketCheckoutDto.UserName, cancellationToken);
        return new CheckoutBasketResult(true);
    }
}