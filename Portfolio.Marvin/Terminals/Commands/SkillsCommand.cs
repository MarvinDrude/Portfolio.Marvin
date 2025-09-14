using Portfolio.Marvin.Terminals.Interfaces;

namespace Portfolio.Marvin.Terminals.Commands;

public sealed class SkillsCommand : ITerminalCommand
{
   public string Name => "skills";
   public string Description => "List of skills & interests";
   
   public bool IsPublic => true;

   public ValueTask Execute(TerminalContext context)
   {
      context.Add("- C#, .NET 8/9/10");
      context.Add("- Source Generators");
      context.Add("- Networking (TCP/UDP, REST APIs, WebSockets)");
      context.Add("- ASP.NET Core, Blazor, EF Core");
      context.Add("- Software Architecture & Simplicity");
      
      return ValueTask.CompletedTask;
   }
}