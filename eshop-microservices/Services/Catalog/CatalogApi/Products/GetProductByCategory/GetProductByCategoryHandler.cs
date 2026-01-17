namespace CatalogApi.Products.GetProductByCategory;

public record GetProductsByCategoryResult(
    IEnumerable<Product> products
);

public record GetProductsByCategoryQuery(string Category) : IQuery<GetProductsByCategoryResult>;

internal class GetProductsByCategoryHandler(IDocumentSession session, ILogger<GetProductsByCategoryHandler> logger): IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
    public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProductByCategoryHandler.Handle called with {@Query}", query);
        var products =  await session.Query<Product>().Where(p => p.Category.Contains(query.Category)).ToListAsync(cancellationToken);
        return new GetProductsByCategoryResult(products);
    }
}