using Portfolio.Marvin.Terminals.Interfaces;

namespace Portfolio.Marvin.Terminals.Commands;

public sealed class HireCommand : ITerminalCommand
{
   public string Name => "hire";
   public string Description => "Hire me";

   public bool IsPublic => true;
   
   public ValueTask Execute(TerminalContext context)
   {
      context.Add("Permission granted. Hiring sequence initiated.");
      
      return ValueTask.CompletedTask;
   }
}