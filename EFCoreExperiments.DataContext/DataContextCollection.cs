using EFCoreExperiments.DataContext.Contexts;
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
}
