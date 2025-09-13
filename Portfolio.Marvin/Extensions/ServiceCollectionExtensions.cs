using Portfolio.Marvin.Providers;
using Portfolio.Marvin.Providers.Interfaces;

namespace Portfolio.Marvin.Extensions;

public static class ServiceCollectionExtensions
{
   public static IServiceCollection AddPortfolioServices(this IServiceCollection services, IConfiguration configuration)
   {
      return services
         .AddProviders(configuration);
   }

   public static IServiceCollection AddProviders(this IServiceCollection services, IConfiguration configuration)
   {
      return services
         .AddSingleton<ITechnologyProvider, TechnologyProvider>();
   }
}