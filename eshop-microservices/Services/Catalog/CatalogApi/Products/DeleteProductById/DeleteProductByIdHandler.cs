namespace CatalogApi.Products.DeleteProductById;

public record DeleteProductByIdResult
(
    Guid Id
);

public record DeleteProductByIdCommand(Guid Id) : ICommand<DeleteProductByIdResult>;

public class DeleteProductByIdCommandValidator : AbstractValidator<DeleteProductByIdCommand>
{
    public DeleteProductByIdCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is Required");
    }
}

public class DeleteProductByIdHandler(IDocumentSession session): ICommandHandler<DeleteProductByIdCommand, DeleteProductByIdResult>
{
    public async Task<DeleteProductByIdResult> Handle(DeleteProductByIdCommand request, CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(request.Id, cancellationToken);
        if (product != null)
        {
            session.Delete<Product>(request.Id);
            await session.SaveChangesAsync(cancellationToken);
        }
        return new DeleteProductByIdResult(request.Id);

    }
}