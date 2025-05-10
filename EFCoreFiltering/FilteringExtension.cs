using EFCoreFiltering.Configurations;
using EFCoreFiltering.Implementations;
using EFCoreFiltering.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EFCoreFiltering;

public static class FilteringExtension
{
    public static IServiceCollection UseFiltering(this IServiceCollection services, IConfiguration configuration, string section = "Filtering")
    {
        services.AddSingleton(configuration.GetSection(section).Get<FilteringConfiguration>());

        services.AddScoped<IRequestQuery, RequestQuery>();

        return services;
    }
}
