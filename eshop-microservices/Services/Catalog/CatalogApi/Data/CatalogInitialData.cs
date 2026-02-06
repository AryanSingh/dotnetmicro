using Marten.Schema;

namespace CatalogApi.Data;

public class CatalogInitialData: IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        using var session = store.LightweightSession();
        if (await session.Query<Product>().AnyAsync()) return;

        session.Store<Product>(GetPreconfiguredProducts());
        await session.SaveChangesAsync();

    }
    private static IEnumerable<Product> GetPreconfiguredProducts()
    {
        return new List<Product>()
        {
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Laptop",
                Description = "A high-performance laptop for all your computing needs.",
                Category = ["Electronics"],
                Price = 999.99m
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Smartphone",
                Description = "A sleek smartphone with the latest features.",
                Category = ["Electronics"],
                Price = 699.99m
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Headphones",
                Category = ["Accesories"],
                Description = "Noise-cancellation headphones",
                Price = 199.99m
            }
        };
    }
}

