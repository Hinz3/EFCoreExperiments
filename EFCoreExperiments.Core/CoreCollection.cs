using EFCoreExperiments.Core.Interfaces.Repositories;
using EFCoreExperiments.Core.Repositories;
using EFCoreFiltering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EFCoreExperiments.Core;

public static class CoreCollection
{
    public static IServiceCollection UseCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.UseFiltering(configuration);

        services.AddScoped<IUserRepository, UserRepository>();
        
        return services;
    }
}
