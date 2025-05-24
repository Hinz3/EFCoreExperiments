using EFCoreExperiments.DataContext.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EFCoreExperiments.DataContext;

public static class DataContextCollection
{
    public static IServiceCollection UseContexts(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MainContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }

    public static IApplicationBuilder ApplyMigrations<T>(this IApplicationBuilder app)
        where T : DbContext
    {
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<T>(); ;

        var migrations = context.Database.GetPendingMigrations();
        if (!migrations.Any())
        {
            return app;
        }

        context.Database.Migrate();
        return app;
    }
}
