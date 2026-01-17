namespace CatalogApi.Products.UpdateProducts;


public record UpdateProductByIdRequest(Product Product);
public record UpdateProductByIdResponse(Product Product);

public class UpdateProductByIdEndpoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/product/{id}/update", async (UpdateProductByIdRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateProductByIdCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<UpdateProductByIdResponse>();
                return Results.Ok(response);
            })
            .WithName("UpdateProductById")
            .Produces<UpdateProductByIdResponse>(StatusCodes.Status200OK)
            .WithSummary("Update Product By Id")
            .WithDescription("Update Product By Id");
    }
}