
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