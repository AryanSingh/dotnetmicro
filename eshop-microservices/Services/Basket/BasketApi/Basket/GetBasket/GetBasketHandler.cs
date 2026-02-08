using BasketApi.Models;

namespace BasketApi.Basket.GetBasket;

public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;



public class GetBasketQueryHandler: IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {

        return new GetBasketResult(new ShoppingCart("aryan"));
    }
}