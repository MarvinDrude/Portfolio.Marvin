
## Introduction

C# 14 introduces extension members ("extension everything"),
not just methods, but also properties, static members, even operators, with nicer grouping and syntax.

Here are some of the most interesting / awesome examples & use-cases, what they enable, plus some quirks. 

## Key Features of Extension Members

Here are what C# 14 lets you do with extension members that you couldn’t (or not cleanly) do before:

- Instance extension properties (in addition to methods) e.g. myList.IsEmpty instead of myList.IsEmpty().
- Static extension members that "live" on the type rather than instances. For example static methods/properties callable like Type.Foo(...).
- Extension operators (e.g. operator +) in static extension context.
- Ref returns of ref structs or its members.
- Grouping of related extension members under a shared receiver via the extension block, 
so less repetition of "this ReceiverType" in each method.

Here are some simple examples of the new syntax (you can add as many extensions in one block of `extension(..)` as you want):

```csharp
public static class ExampleExtensions
{
   extension<T>(scoped in Span<T> span)
   {
      /// Example of get only properties as extension
      public ref T LastItem => ref span[^1];
      public ref T FirstItem => ref span[0];
   }

   // example of extending any unmanged type
   extension<T>(T value)
      where T : unmanaged
   {
      // new method as extension
      public int WriteBigEndian(scoped Span<byte> buffer)
      {
         var size = Unsafe.SizeOf<T>();
         buffer = buffer[..size];
         
         MemoryMarshal.Write(buffer, in value);
         if (BitConverter.IsLittleEndian) buffer.Reverse();
         
         return size;
      }
   }
   
   extension<T>(T value)
   {
      // example if you wanted any type to have a deserialize method staticly
      public static T DeserializeMemory(scoped in ReadOnlyMemory<byte> memory)
      {
         return SerializerCache<T>.DeserializeMemory(memory.Span);       
      }
   }
}
```

## More Complex Example with ``ref struct`` Builders

When working on a low-allocation code generation library (designed for source generators in .NET 10 and beyond), 
one of the biggest challenges I ran into was enabling a fluent builder pattern with ``ref structs``

Normally, builder patterns are straightforward: each method modifies the builder's state and 
then returns ``this`` so that calls can be chained fluently. With ``ref structs``, however, this is not trivial. 
Since ``ref structs`` cannot implement inheritance hierarchies, and since they must be passed by reference 
to avoid unnecessary copying, creating a shared, composable API across multiple builders becomes tricky.

For example, in a code generation scenario you might have different specialized builders:
- ``FileBuilder`` → for writing an entire source file.
- ``TypeHeaderBuilder`` → for building class, struct, or interface headers.
- ``MethodBuilder``, ``NamespaceBuilder``, etc.

Each of these builders should expose the same set of common methods such as ``WriteLine``, ``Write``, ``Indent``, or ``Unindent``. I
n a class hierarchy, you would solve this by placing shared logic in a base class. With ``ref structs``, however, 
inheritance is off the table.

This is exactly where the new C# 14 extension blocks shine. Instead of duplicating boilerplate code in every builder,
we can write the common methods once and extend all builder structs in a type-safe and allocation-free manner.

### Defining a Shared Interface

The first step is to define an interface that all builder structs implement. 
This allows us to define a common contract while still keeping the performance and safety guarantees of ``ref struct``.

```csharp
public interface ICodeBuilder
{
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   internal ref CodeBuilder GetBuilder();
}
```

Each builder (``FileBuilder``, ``TypeHeaderBuilder``, …) implements ``ICodeBuilder`` 
by returning a reference to its underlying ``CodeBuilder``, which owns the actual writer state.

### Extension Blocks for Fluent APIs

Now comes the interesting part. Instead of writing ``WriteLine`` and ``Write`` in every single builder, 
we use an extension block with the new ``extension<T>(ref T)`` syntax:

```csharp
public static class CodeBuilderExtensions
{
   extension<T>(ref T builder)
      where T : struct, ICodeBuilder, allows ref struct
   {
      // ... more methods 
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public ref T WriteLine(string line)
      {
         ref var writer = ref builder.GetBuilder().Writer;
         writer.WriteLine(line);
         
         return ref builder;
      }
      
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public ref T WriteLine()
      {
         ref var writer = ref builder.GetBuilder().Writer;
         writer.WriteLine();
         
         return ref builder;
      }
      // ... more methods like UpIndent, DownIndent etc.
   }
}
```

### Fluent Usage

The result is a clean and ergonomic API that looks like a normal 
builder pattern but internally is optimized for **high performance** and **low GC pressure**:

```csharp
builder.File
   .WriteStartAutoGenerated()
      .WriteLine("// Here")
      .WriteLine("// is a message")
      .WriteLineInterpolated($"// Test {x}")
   .WriteEndAutoGenerated()
   .WriteNullableEnable()
      .WriteUsing("NameSpaceA")
      .WriteUsing("NameSpaceB.Test", true);
```

With this design, adding new builders or extending existing ones requires zero redundant boilerplate, 
while still leveraging the unique power of ``ref struct`` for performance-critical scenarios like code generation.

👉 If you are interested in the full source generation builder, you can check it out here:
[CodeGen.Core](https://github.com/MarvinDrude/CodeGen.Core)