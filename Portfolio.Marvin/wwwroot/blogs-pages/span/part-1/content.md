
## Introduction

High-performance applications in .NET often face the same enemy: **garbage collection (GC) pressure**. 
Every string split, array copy, or memory slice can add up, causing allocations that choke throughput in hot paths.

With the rise of `Span<T>`, `Memory<T>`, and related low-level APIs, .NET developers have the tools to eliminate most day to day heap allocations, 
achieving near zero-allocation code in many scenarios.

In the following, we will take a dive into some of the basics and some more advanced examples:

- What `Span<T>` and `Memory<T>` really are.
- How they differ from arrays and strings.
- How to build APIs that don’t allocate.
- Real-world examples with parsers, encoders, and buffers.
- Custom allocators using `ArrayPool<T>` and `MemoryPool<T>`.
- A glimpse into **.NET 10 enhancements** for spans and memory.

## Why care are about Zero-Allocation anyway?

Every time the GC runs, it pauses your application to collect unused memory. 
While .NET’s GC is world-class, in **low-latency, high-throughput systems** 
(like databases, messaging systems, telemetry collectors, or real-time APIs), 
unnecessary allocations translate into latency spikes.

By reducing allocations:

- Throughput increases.
- Latency becomes predictable.
- Cache locality improves.

## Span<T>: The Gateway to Zero-Allocation

At its core, `Span<T>` provides a type-safe and memory-safe representation of a contiguous region of arbitrary memory.
Since it is a **ref struct** it can only live on the stack, meaning you cannot store it in class fields for example.

The code below is a very simple example of how we could create a temporary stack allocated int array compared
to the heap allocation.

```csharp
// stack allocated
Span<int> numbers = stackalloc int[4] { 1, 2, 3, 4 };
// heap allocated
Span<int> numbersHeap = new int[4] { 1, 2, 3, 4 };

// method slice
var slice = numbersHeap.Slice(2, 2);
// indexer slice
var anotherSlice = numbers[2..];
```

### Key points:

- `Span<T>` cannot escape to the heap
- It can point to heap memory, stack memory or unmanaged memory
- Slicing operations do **not allocate** new heap memory

## Strings: a very common example

One of the biggest sources of hidden allocations in .NET of the average companies code will probably be **string manipulation**.
Methods like `Split`, `Substring`, or `Replace` often allocate new strings behind the scenes, even if we only need to peek at a small part of the input.

To illustrate this, let’s benchmark a simple case: extracting a user ID from a `USERNAME|ID` formatted string.  
We’ll compare the **naive approach** using `Split` with a **Span-based approach** that avoids allocations entirely.

Both methods first get the ID from the string and then lookup a value inside a dictionary.
(We don't care for checking bounds for this example)
The naive approach that can be found in so many code bases would be to just have the following:

```csharp
private readonly Dictionary<string, string> _usersPerId = [];
private const string _example = "USERNAME:marvin|ID:testid";
```

```csharp
[Benchmark]
public string NaiveExample()
{
   var parts = _example.Split('|');
   var userId = parts[1].Split(':')[1];
  
   return _usersPerId[userId];
}
```

For this case, every call to `Split` will allocate **two new strings on the heap**.  
Even in such a simple example, this can add up quickly in hot paths.

Fortunately, we can rewrite the code to **completely avoid heap allocations**.  
By using the newer `Dictionary<TKey, TValue>.GetAlternateLookup<T>()`, 
we don’t even need to create an intermediate string for the dictionary lookup anymore.

```csharp
[Benchmark]
public string SpanExample()
{
   ReadOnlySpan<char> span = _example;
  
   var idPart = span[(span.IndexOf('|') + 1)..];
   var userId = idPart[(idPart.IndexOf(':') + 1)..];
  
   var alternateLookup = _usersPerId.GetAlternateLookup<ReadOnlySpan<char>>();
   var result = alternateLookup[userId];
  
   return result;
}
```

### Benchmark Results
The impact is immediately clear:

<div class="table-wrapper">

| Method       | N    | Mean     | Error     | StdDev   | Min      | Max      | Median   | Gen0   | Allocated |
|------------- |----- |---------:|----------:|---------:|---------:|---------:|---------:|-------:|----------:|
| NaiveExample | 10   | 58.64 ns |  0.367 ns | 0.131 ns | 58.46 ns | 58.78 ns | 58.66 ns | 0.0049 |     248 B |
| SpanExample  | 10   | 17.38 ns |  0.589 ns | 0.210 ns | 17.11 ns | 17.67 ns | 17.36 ns |      - |         - |
| NaiveExample | 100  | 59.37 ns |  3.276 ns | 1.168 ns | 58.33 ns | 61.13 ns | 58.90 ns | 0.0049 |     248 B |
| SpanExample  | 100  | 15.07 ns |  2.069 ns | 0.738 ns | 13.70 ns | 15.84 ns | 15.18 ns |      - |         - |
| NaiveExample | 1000 | 73.12 ns | 10.299 ns | 3.673 ns | 68.31 ns | 78.71 ns | 73.09 ns | 0.0049 |     248 B |
| SpanExample  | 1000 | 14.38 ns |  2.212 ns | 0.789 ns | 13.59 ns | 15.28 ns | 14.31 ns |      - |         - |

</div>

## ArrayPool<T> and MemoryPool<T>

`Span<T>` is fantastic for working with small slices, especially when you can leverage `stackalloc` for short-lived buffers.  
However, stack space is limited, and once your buffers grow beyond a few kilobytes, heap allocations become unavoidable.

This is where **buffer pooling** comes in. Instead of constantly allocating and freeing large arrays on the heap 
(which puts stress on the GC),  
you can rent and reuse memory from specialized pools.

### ArrayPool<T>

`ArrayPool<T>` is a high-performance, shared pool for arrays.  
It lets you "rent" an array for temporary use and then "return" it when you’re done.  
This way, the array can be recycled instead of being garbage collected.

```csharp
using System.Buffers;

public class ArrayPoolExample
{
   public void ProcessLargeData()
   { 
      const int size = 1024 * 1024; // 1 MB
      var pool = ArrayPool<byte>.Shared;

      // Rent a buffer from the pool
      byte[] buffer = pool.Rent(size);

      try
      {
         // Use it as a Span<T> without allocations
         var span = buffer.AsSpan(0, size);

         // Do some work
         span.Fill(42);
      }
      finally
      {
         // Important: always return the buffer
         pool.Return(buffer);
      }
   }
}
```

✅ Pros:

- Extremely fast.
- Great for temporary array usage.
- Shared across the entire process.

⚠️ Caveats:

- Arrays can be larger than requested (`Rent` may overshoot).
- Buffers are not cleared by default (data leakage if not overwritten).

### MemoryPool<T>

For scenarios involving streaming, I/O pipelines, or very large data buffers, `MemoryPool<T>` is a more advanced option.
It provides `IMemoryOwner<T>` handles that can be disposed safely, and integrates naturally with `Memory<T>` and `Span<T>`.

```csharp
public class MemoryPoolExample
{
   public void UseMemoryPool()
   {
      using IMemoryOwner<byte> owner = MemoryPool<byte>.Shared.Rent(4096);
      Memory<byte> memory = owner.Memory;

      // Access as Span<T>
      var span = memory.Span;
      span[0] = 99;

      // Pass Memory<T> to async APIs
      SomeAsyncApi(memory);
   }

   private void SomeApi(Memory<byte> buffer)
   {
      // Example placeholder for I/O API accepting Memory<T>
   }
}
```

✅ Pros:

- Integrates with ``Memory<T>`` and APIs.
- Safe lifetime management via ``IMemoryOwner<T>``

⚠️ Caveats:

- Slightly more overhead than ``ArrayPool<T>``

## Enhancements coming in .NET 10

Up until now, `Span<T>` and `ReadOnlySpan<T>` have been incredibly powerful, 
but the C# compiler treated them as "second-class citizens". 
They worked well in APIs that explicitly targeted them, but required lots of boilerplate overloads and 
explicit conversions to cover arrays, strings, and spans.

Starting with **C# 14 / .NET 10**, spans become **first-class language citizens**.  
This means the compiler understands them as naturally as arrays and strings, making code more intuitive and eliminating many duplicate APIs.

### Key Improvements

- **Implicit conversions everywhere**
- `T[]` → `Span<T>`
- `T[]` → `ReadOnlySpan<T>`
- `Span<T>` → `ReadOnlySpan<T>`
- `string` → `ReadOnlySpan<char>`

<br/>
No need to call `.AsSpan()` or provide extra overloads anymore.
<br/>

- Better type inference
- Generics that work with spans and arrays now infer types more naturally.
- Example: methods that accept `ReadOnlySpan<T>` will accept arrays without ambiguity or explicit type parameters.

### Why this matters

- Cleaner APIs with fewer overloads.
- Less ceremony (`.AsSpan()` everywhere is no longer needed).
- Safer overload resolution that aligns with how the runtime actually optimizes spans.
- A future where spans integrate more seamlessly with collections, UTF-8 strings, and async streaming APIs.