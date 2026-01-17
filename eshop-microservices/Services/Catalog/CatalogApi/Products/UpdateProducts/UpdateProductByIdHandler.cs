namespace CatalogApi.Products.UpdateProducts;

public record UpdateProductByIdResult(Product Product);

public record UpdateProductByIdCommand(Product Product)
    : ICommand<UpdateProductByIdResult>;
public class UpdateProductByIdQueryHandler(IDocumentSession session, ILogger<UpdateProductByIdQueryHandler> logger): ICommandHandler<UpdateProductByIdCommand, UpdateProductByIdResult>
{
    public async Task<UpdateProductByIdResult> Handle(UpdateProductByIdCommand command, CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(command.Product.Id, cancellationToken);
        if (product == null)
        {
            throw new ProductNotFoundException();
        }
        product.Name = command.Product.Name;
        product.Description = command.Product.Description;
        product.Category = command.Product.Category;
        product.Price = command.Product.Price;
        
        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);
        return new UpdateProductByIdResult(product);
    }
}