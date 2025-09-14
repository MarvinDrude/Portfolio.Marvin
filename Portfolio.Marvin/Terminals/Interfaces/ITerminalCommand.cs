namespace Portfolio.Marvin.Terminals.Interfaces;

public interface ITerminalCommand
{
   public string Name { get; }
   
   public string Description { get; }
   
   public bool IsPublic { get; }

   public ValueTask Execute(TerminalContext context);
}