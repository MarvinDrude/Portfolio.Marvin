using System.Runtime.CompilerServices;
using Me.Memory.Buffers;

namespace Portfolio.Marvin.Terminals;

public sealed class TerminalContext
{
   public required string RawText { get; set; }
   
   public required CircularBuffer<string> Buffer { get; init; }

   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public void Add(string line)
   {
      Buffer.Add(line);
   }
}