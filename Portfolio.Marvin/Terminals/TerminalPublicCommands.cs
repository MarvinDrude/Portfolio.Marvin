using Portfolio.Marvin.Terminals.Interfaces;

namespace Portfolio.Marvin.Terminals;

public sealed class TerminalPublicCommands
{
   public Dictionary<string, ITerminalCommand> Commands { get; } = [];

   public TerminalPublicCommands(IEnumerable<ITerminalCommand> commands)
   {
      foreach (var command in commands.Where(x => x.IsPublic))
      {
         Commands[command.Name] = command;
      }
   }
}