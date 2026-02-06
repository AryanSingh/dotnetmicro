namespace CatalogApi.Products.UpdateProducts;

public record UpdateProductByIdResult(Product Product);

public record UpdateProductByIdCommand(Product Product)
    : ICommand<UpdateProductByIdResult>;

public class UpdateProductByIdCommandValidator: AbstractValidator<UpdateProductByIdCommand>
{
    public UpdateProductByIdCommandValidator()
    {
        RuleFor(x => x.Product.Id).NotEmpty().WithMessage("Id is Required");
        RuleFor(x => x.Product.Name).NotEmpty().WithMessage("Name is Required");
        RuleFor(x => x.Product.Category).NotEmpty().WithMessage("Category is Required");
        RuleFor(x => x.Product.Price).GreaterThan(0).WithMessage("Price must be greater than zero");
    }
}
public class UpdateProductByIdCommandHandler(IDocumentSession session): ICommandHandler<UpdateProductByIdCommand, UpdateProductByIdResult>
{
    public async Task<UpdateProductByIdResult> Handle(UpdateProductByIdCommand command, CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(command.Product.Id, cancellationToken);
        if (product == null)
        {
            throw new ProductNotFoundException(command.Product.Id);
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