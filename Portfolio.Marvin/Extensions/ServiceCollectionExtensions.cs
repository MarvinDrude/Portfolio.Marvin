using Portfolio.Marvin.Providers;
using Portfolio.Marvin.Providers.Interfaces;
using Portfolio.Marvin.Terminals;
using Portfolio.Marvin.Terminals.Commands;
using Portfolio.Marvin.Terminals.Interfaces;

namespace Portfolio.Marvin.Extensions;

public static class ServiceCollectionExtensions
{
   public static IServiceCollection AddPortfolioServices(this IServiceCollection services, IConfiguration configuration)
   {
      return services
         .AddProviders(configuration)
         .AddTerminalCommands(configuration);
   }

   public static IServiceCollection AddProviders(this IServiceCollection services, IConfiguration configuration)
   {
      return services
         .AddSingleton<ITechnologyProvider, TechnologyProvider>()
         .AddSingleton<IExperienceProvider, ExperienceProvider>()
         .AddSingleton<IProjectProvider, ProjectProvider>()
         .AddSingleton<IBlogProvider, BlogProvider>();
   }

   public static IServiceCollection AddTerminalCommands(this IServiceCollection services, IConfiguration configuration)
   {
      return services
         .AddSingleton<TerminalCommandHandler>()
         .AddSingleton<TerminalCommandRegistry>()
         .AddSingleton<TerminalPublicCommands>()
         .AddSingleton<ITerminalCommand, HelpCommand>()
         .AddSingleton<ITerminalCommand, SkillsCommand>()
         .AddSingleton<ITerminalCommand, NiklasCommand>()
         .AddSingleton<ITerminalCommand, LukasCommand>()
         .AddSingleton<ITerminalCommand, HireCommand>();
   }
}