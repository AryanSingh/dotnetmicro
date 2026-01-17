namespace CatalogApi.Products.DeleteProductById;


public record DeleteProductByIdResponse(Guid Id);

public class DeleteProductByIdEndpoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/product/{id}", async (Guid id, ISender sender) =>
            {
                var command = new DeleteProductByIdCommand(id);
                var result = await sender.Send(command);
                var response = result.Adapt<DeleteProductByIdResponse>();
                return Results.Ok(response);
            })
            .WithName("DeleteProduct")
            .WithDescription("Product deleted")
            .Produces<DeleteProductByIdResponse>(StatusCodes.Status200OK)
            .WithSummary("Deleted Product");

    }
}