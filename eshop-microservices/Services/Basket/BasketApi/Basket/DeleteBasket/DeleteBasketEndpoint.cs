namespace BasketApi.Basket.DeleteBasket;

public record DeleteBasketResponse(bool IsSuccess);
// public record DeleteBasketCommand(string UserName);

public class DeleteBasketEndpoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{userName}", async (string userName, ISender sender) =>
        {
            var command = await sender.Send(new DeleteBasketCommand(userName));
            var result = sender.Send(command);
            var response = result.Adapt<DeleteBasketResponse>();
            return Results.Ok(response);
        })
        .WithName("DeleteBasket")
        .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Delete Basket")
        .WithDescription("Delete Basket");
    }
}