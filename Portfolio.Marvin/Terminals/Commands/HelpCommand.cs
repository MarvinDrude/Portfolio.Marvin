using Portfolio.Marvin.Terminals.Interfaces;

namespace Portfolio.Marvin.Terminals.Commands;

public sealed class HelpCommand : ITerminalCommand
{
   public string Name => "help";
   public string Description => "Displays help";
   public bool IsPublic => false;

   private readonly IServiceProvider _serviceProvider;
   
   public HelpCommand(IServiceProvider provider)
   {
      _serviceProvider = provider;
   }
   
   public ValueTask Execute(TerminalContext context)
   {
      var publicCommands = _serviceProvider.GetRequiredService<TerminalPublicCommands>()
         .Commands.Values.OrderBy(x => x.Name);

      context.Add("Available commands: ");

      foreach (var command in publicCommands)
      {
         context.Add($"{command.Name} - {command.Description}");
      }
      
      return ValueTask.CompletedTask;
   }
}