
namespace BasketApi.Exceptions;

public class BasketNotFoundException: NotFoundException
{
    public BasketNotFoundException(string userName) : base("Basket", userName)
    {
    }

    public BasketNotFoundException(string name, object key) : base(name, key)
    {
    }
}