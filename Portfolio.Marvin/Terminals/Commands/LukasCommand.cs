using Portfolio.Marvin.Terminals.Interfaces;

namespace Portfolio.Marvin.Terminals.Commands;

public sealed class LukasCommand : ITerminalCommand
{
   public string Name => "lukas";
   public string Description => "It is lukas...";

   public bool IsPublic => false;
   
   public ValueTask Execute(TerminalContext context)
   {
      context.Add("Ew, lukas...");
      
      return ValueTask.CompletedTask;
   }
}