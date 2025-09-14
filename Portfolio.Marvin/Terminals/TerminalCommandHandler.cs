using System.Net;

namespace Portfolio.Marvin.Terminals;

public sealed class TerminalCommandHandler
{
   private readonly TerminalCommandRegistry _registry;
   
   public TerminalCommandHandler(TerminalCommandRegistry registry)
   {
      _registry = registry;
   }
   
   public async Task Execute(TerminalContext context)
   {
      var rawText = context.RawText;
      var commandName = rawText;
      
      if (rawText.IndexOf(' ') is var index and >= 0)
      {
         commandName = rawText[..index];
      }

      if (_registry.Get(commandName) is { } command)
      {
         await command.Execute(context);
         return;
      }

      context.Add($"<span style='color: var(--error-color)'>Could not find command '{commandName}'</span>");
   }
}