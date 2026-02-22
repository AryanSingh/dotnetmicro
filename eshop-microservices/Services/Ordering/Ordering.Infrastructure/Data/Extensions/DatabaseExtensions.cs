using Microsoft.AspNetCore.Builder;

namespace Ordering.Infrastructure.Data.Extensions;

public static  class DatabaseExtensions
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.MigrateAsync().GetAwaiter().GetResult();
        await SeedAsync(context);

    }

    private static async Task SeedAsync(ApplicationDbContext context)
    {
        if (!await context.Customers.AnyAsync())
        {
            await context.Customers.AddRangeAsync(InitialData.Customers);
            await context.SaveChangesAsync();
        }
        if(!await context.Products.AnyAsync())
        {
            await context.Products.AddRangeAsync(InitialData.Products);
            await context.SaveChangesAsync();
        }

        if (!await context.Orders.AnyAsync())
        {
            await context.Orders.AddRangeAsync(InitialData.Orders);
            await context.SaveChangesAsync();
        }
        
        
    }
}