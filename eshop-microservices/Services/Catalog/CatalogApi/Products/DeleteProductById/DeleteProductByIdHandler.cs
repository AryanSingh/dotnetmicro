namespace CatalogApi.Products.DeleteProductById;

public record DeleteProductByIdResult
(
    Guid Id
);

public record DeleteProductByIdCommand(Guid Id) : ICommand<DeleteProductByIdResult>;

public class DeleteProductByIdHandler(IDocumentSession session): ICommandHandler<DeleteProductByIdCommand, DeleteProductByIdResult>
{
    public async Task<DeleteProductByIdResult> Handle(DeleteProductByIdCommand request, CancellationToken cancellationToken)
    {
        session.Delete<Product>(request.Id);
        await session.SaveChangesAsync(cancellationToken);
        return new DeleteProductByIdResult(request.Id);

    }
}