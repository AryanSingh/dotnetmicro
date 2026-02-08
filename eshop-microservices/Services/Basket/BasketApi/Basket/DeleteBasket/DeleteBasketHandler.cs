namespace BasketApi.Basket.DeleteBasket;

public record DeleteBasketCommand(string UserName) : ICommand<DeleteBasketResult>;
public record DeleteBasketResult(bool Success);

public class DeleteCommandValidator: AbstractValidator<DeleteBasketCommand>
{
    public DeleteCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName cannot be empty");
    }
}

public class DeleteBasketHandler: ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        string userName = command.UserName;
        return new DeleteBasketResult(true);
    }
}
