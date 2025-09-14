using Portfolio.Marvin.Terminals.Commands;
using Portfolio.Marvin.Terminals.Interfaces;

namespace Portfolio.Marvin.Terminals;

public sealed class TerminalCommandRegistry
{
   private readonly Dictionary<string, ITerminalCommand> _commands = new (StringComparer.OrdinalIgnoreCase);

   public TerminalCommandRegistry(IEnumerable<ITerminalCommand> commands)
   {
      foreach (var command in commands)
      {
         _commands[command.Name] = command;
      }
   }
   
   public ITerminalCommand? Get(string name)
   {
      return _commands.GetValueOrDefault(name);
   }
}