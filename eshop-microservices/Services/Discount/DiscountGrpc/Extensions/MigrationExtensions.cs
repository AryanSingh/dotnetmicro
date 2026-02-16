using DiscountGrpc.Data;
using Microsoft.EntityFrameworkCore;

namespace DiscountGrpc;

public static class MigrationExtensions
{
    public static WebApplication UseMigration(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<DiscountContext>();
        dbContext.Database.Migrate();

        return app;
    }
}
