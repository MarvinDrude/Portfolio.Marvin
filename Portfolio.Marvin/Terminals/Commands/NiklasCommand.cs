using Portfolio.Marvin.Terminals.Interfaces;

namespace Portfolio.Marvin.Terminals.Commands;

public sealed class NiklasCommand : ITerminalCommand
{
   public string Name => "niklas";
   public string Description => "It is niklas...";

   public bool IsPublic => false;
   
   public ValueTask Execute(TerminalContext context)
   {
      context.Add("Who? ... :*");
      
      return ValueTask.CompletedTask;
   }
}