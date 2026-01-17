namespace CatalogApi.Products.GetProducts;


//public record GetProductsRequest()

public record GetProductsResponse(IEnumerable<Product> Products);

public class GetProductsEndpoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (ISender sender) =>
            {
                var result = await sender.Send(new GetProductsQuery());
                var response = result.Adapt<GetProductsResponse>();
                return Results.Ok(response);
            })
            .WithDescription("Get Products")
            .Produces<GetProductsResponse>(StatusCodes.Status200OK)
            .WithName("GetProducts")
            .WithSummary("Get Products")
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}