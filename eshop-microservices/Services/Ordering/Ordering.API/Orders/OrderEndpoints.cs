using BuildingBlocks.Pagination;
using Carter;
using MediatR;
using Ordering.Application.Dtos;
using Ordering.Application.Orders.Commands.CreateOrder;
using Ordering.Application.Orders.Commands.DeletOrder;
using Ordering.Application.Orders.Commands.UpdateOrder;
using Ordering.Application.Orders.Queries.GetOrders;
using Ordering.Application.Orders.Queries.GetOrdersByCustomer;
using Ordering.Application.Orders.Queries.GetOrdersByName;

namespace Ordering.API.Orders;

public record GetOrdersRequest(int PageIndex = 0, int PageSize = 10);
public record CreateOrderRequest(OrderDto Order);
public record UpdateOrderRequest(OrderDto Order);

public class OrderEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders", async ([AsParameters] GetOrdersRequest request, ISender sender) =>
            {
                var query = new GetOrdersQuery(new PaginationRequest(request.PageIndex, request.PageSize));
                var result = await sender.Send(query);
                return Results.Ok(result);
            })
            .WithName("GetOrders")
            .WithSummary("Get paginated orders")
            .Produces<GetOrdersResult>()
            .ProducesProblem(StatusCodes.Status400BadRequest);

        app.MapGet("/orders/by-name/{name}", async (string name, ISender sender) =>
            {
                var result = await sender.Send(new GetOrdersByNameQuery(name));
                return Results.Ok(result);
            })
            .WithName("GetOrdersByName")
            .WithSummary("Get orders filtered by name")
            .Produces<GetOrdersByNameResult>()
            .ProducesProblem(StatusCodes.Status400BadRequest);

        app.MapGet("/orders/by-customer/{customerId:guid}", async (Guid customerId, ISender sender) =>
            {
                var result = await sender.Send(new GetOrdersByCustomerQuery(customerId));
                return Results.Ok(result);
            })
            .WithName("GetOrdersByCustomer")
            .WithSummary("Get orders by customer id")
            .Produces<GetOrdersByCustomerResult>()
            .ProducesProblem(StatusCodes.Status400BadRequest);

        app.MapPost("/orders", async (CreateOrderRequest request, ISender sender) =>
            {
                var result = await sender.Send(new CreateOrderCommand(request.Order));
                return Results.Created($"/orders/{result.Id}", result);
            })
            .WithName("CreateOrder")
            .WithSummary("Create an order")
            .Produces<CreateOrderResult>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        app.MapPut("/orders", async (UpdateOrderRequest request, ISender sender) =>
            {
                var result = await sender.Send(new UpdateOrderCommand(request.Order));
                return Results.Ok(result);
            })
            .WithName("UpdateOrder")
            .WithSummary("Update an order")
            .Produces<UpdateOrderResult>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapDelete("/orders/{orderId:guid}", async (Guid orderId, ISender sender) =>
            {
                await sender.Send(new DeleteOrderCommand(orderId));
                return Results.NoContent();
            })
            .WithName("DeleteOrder")
            .WithSummary("Delete an order")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
