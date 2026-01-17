
namespace CatalogApi.Products.CreateProduct;


public record CreateProductRequest
(
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price
);

public record CreateProductResponse
(
    Guid Id
);
public class CreateProductEndpoint : ICarterModule
{

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
            {
                Console.WriteLine("hitting here");

                var command = request.Adapt<CreateProductCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<CreateProductResponse>();
                return Results.Created($"/products/{response.Id}", response);
            })
            .WithName("CreateProduct")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .WithSummary("Create product")
            .WithDescription("Create product");

        // app.MapGet("/products", () =>
        // {
        //     
        //     return Results.Ok(new { message = "Products endpoint is working. Use POST to create a product." });
        // })
        // .WithName("GetProducts")
        // .Produces(StatusCodes.Status200OK)
        // .WithSummary("Get products info")
        // .WithDescription("Get products endpoint information");
        //
        // app.MapGet("/", () =>
        // {
        //     return Results.Ok("Catalog API is running...");
        // });
    }

}