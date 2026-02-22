using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Extensions;

internal class InitialData
{
    private static Address Address1 => Address.Of("Aryan", "Singh", "aryan@yopmail.com", "My Street", "India", "UP", "201301");
    private static Address Address2 => Address.Of("Jane", "Doe", "jane@yopmail.com", "Her Street", "USA", "NY", "10001");
    private static Payment Payment1 => Payment.Of("1234123412341234", "Aryan Singh", DateTime.UtcNow.AddYears(1), "123", 1);
    private static Payment Payment2 => Payment.Of("4321432143214321", "Jane Doe", DateTime.UtcNow.AddYears(2), "321", 2);

    public static IEnumerable<Customer> Customers =>
        new List<Customer>
        {
            Customer.Create(CustomerId.Of(Guid.Parse("58c49479-ec65-4de2-86e7-033c546291aa")), "Customer 1", "random@yopmail.com"),
            Customer.Create(CustomerId.Of(Guid.Parse("c0e86b0f-8d76-4be0-83ad-d0074dbfb002")), "Customer 2", "random2@yopmail.com")
        };
    
    public static IEnumerable<Product> Products => 
        new List<Product>
        {
            Product.Create(ProductId.Of(Guid.Parse("5334c996-8457-4cf0-815c-ed2b77c4ff61")), "Product 1", 10.00m),
            Product.Create(ProductId.Of(Guid.Parse("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914")), "Product 2", 20.00m),
            Product.Create(ProductId.Of(Guid.Parse("17d235c5-e51c-43d9-93e1-2fb11d4e0b04")), "Product 3", 30.00m),
            Product.Create(ProductId.Of(Guid.Parse("370604b9-eb39-4475-802c-4903ec41fc32")), "Product 4", 40.00m)
        };
    
    public static IEnumerable<Order> Orders =>
        new List<Order>
        {
            GetDefaultOrder1(),
            GetDefaultOrder2()
        };

    private static Order GetDefaultOrder1()
    {
        var order = Order.Create(
            OrderId.Of(Guid.Parse("d7fb44cf-1abc-4277-bfdd-eb6d4b2e8fb4")),
            CustomerId.Of(Guid.Parse("58c49479-ec65-4de2-86e7-033c546291aa")),
            OrderName.Of("O0001"),
            Address1,
            Address1,
            Payment1
        );
        order.Add(ProductId.Of(Guid.Parse("5334c996-8457-4cf0-815c-ed2b77c4ff61")), 2, 10.00m);
        order.Add(ProductId.Of(Guid.Parse("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914")), 1, 20.00m);
        return order;
    }

    private static Order GetDefaultOrder2()
    {
        var order = Order.Create(
            OrderId.Of(Guid.Parse("b118b6e3-2e2d-4874-a690-335bedc9fbf8")),
            CustomerId.Of(Guid.Parse("c0e86b0f-8d76-4be0-83ad-d0074dbfb002")),
            OrderName.Of("O0002"),
            Address2,
            Address2,
            Payment2
        );
        order.Add(ProductId.Of(Guid.Parse("17d235c5-e51c-43d9-93e1-2fb11d4e0b04")), 1, 30.00m);
        order.Add(ProductId.Of(Guid.Parse("370604b9-eb39-4475-802c-4903ec41fc32")), 3, 40.00m);
        return order;
    }
}