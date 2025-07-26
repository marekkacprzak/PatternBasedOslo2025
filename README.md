# C# Pattern-Based Development Examples

This project demonstrates various pattern-based programming concepts in C# and with .NET 9.0, showcasing modern C# features and custom implementations.

## Overview

The solution contains examples of:
- Custom Awaiter patterns
- Custom Enumerator patterns (struct-based and async)
- Source generator implementation for automatic Deconstruct method generation
- Collection expressions and collection builders
- Various enumeration patterns

## Project Structure

### Main Components

1. **Generator Project** - Source generator for automatic `Deconstruct` method generation
2. **GeneratorContract** - Attribute definitions for the source generator
3. **ConsoleApp1** - Main application with examples

## Examples Demonstrated

### 1. Custom Awaiter Pattern (`SampleWithAwaitTimeSpan`)

```csharp
var delay = await TimeSpan.FromSeconds(2);
```

- **File**: `SampleWithAwaitTimeSpan.cs`
- **Shows**: Custom `GetAwaiter()` implementation for `TimeSpan`
- **Pattern**: Implements `ICriticalNotifyCompletion` (C# 5.0) to make `TimeSpan` awaitable
- **Result**: You can directly await a `TimeSpan` which delays execution and returns the original timespan value

### 2. Custom Async Enumerator Pattern (`SampleWithAwaiter`)

```csharp
await foreach(var item in ourListWithoutEnumeratorButWithAsync[1..^2])
{
    Console.WriteLine(item);
}
```

- **File**: `SampleWithAwaiter.cs`
- **Shows**: Custom async enumeration (C# 8.0) with slicing support
- **Pattern**: `GetAsyncEnumerator()` returning custom `OurEnumeratorWithAsync`
- **Features**: Async enumeration with delay, range/slice support (C# 8.0)

### 3. Struct-Based Enumerator Pattern (`SampleEnumeratorAsStruct`)

```csharp
foreach (ref var item in ourListWithoutEnumerator[1..^2])
{
    Console.WriteLine(item);
}
```

- **File**: `SampleEnumeratorAsStruct.cs`
- **Shows**: Zero-allocation enumeration using `ref struct` (C# 7.2)
- **Pattern**: Custom `GetEnumerator()` returning `ref struct` enumerator
- **Benefits**: No heap allocations, `ref` access to elements (C# 7.0), range/slice support (C# 8.0)

### 4. Source Generator for Deconstruct Methods

```csharp
[GeneratedDeconstruct]
public sealed partial class Talk
{
    public Talk(string title, string conference, DateTimeOffset start) { ... }
    public Talk(bool active, string title) { ... }
}

// Usage:
(string title, string conference, DateTimeOffset start) = talk;
(bool active, string titleNew) = talk2;
```

- **Files**: `DeconstructGenerator.cs`, `Talk.cs`
- **Shows**: Automatic generation of `Deconstruct` methods based on constructor parameters
- **Pattern**: Analyzes constructor body assignments and generates appropriate deconstruction methods using Source Generators (C# 9.0)
- **Features**: Supports multiple constructors, maps constructor parameters to properties

## Collection Types Demonstrated

### 1. `OurList<T>` - Basic Custom Collection
- Implements `IEnumerable<T>`
- Collection initializer support
- Multiple `Add` method overloads

### 2. `OurListWithSpan<T>` - Collection with Range Support
- Collection expressions support (C# 12.0): `[1, 2, 3]` syntax
- Range/slice operations (C# 8.0): `collection[1..^2]`
- `CollectionBuilder` attribute usage (C# 12.0)

### 3. `OurListWithoutEnumerator<T>` - Struct Enumerator
- `ref struct` enumerator (C# 7.2) for zero allocations
- `ref` return values (C# 7.0) for direct element access
- Custom `Dispose` implementation

### 4. `OurListWithoutEnumeratorButWithAsync<T>` - Async Enumeration
- Both sync and async enumeration support
- Custom async enumerator (C# 8.0) with delay simulation
- Proper `ValueTask` based async disposal (C# 7.0/8.0)

## Key C# Features Showcased

- **Collection Expressions** (C# 12.0): `[1, 2, 3]` syntax
- **Range/Index Operators** (C# 8.0): `^1`, `1..^2`
- **Pattern Matching** (C# 7.0+): Property patterns (C# 8.0), nested patterns (C# 8.0)
- **Custom Awaiters** (C# 5.0): Making any type awaitable
- **Source Generators** (C# 9.0): Compile-time code generation
- **ref struct** (C# 7.2): Stack-only types for performance
- **Collection Builders** (C# 12.0): Custom collection initialization with `CollectionBuilderAttribute`
- **Async Enumeration** (C# 8.0): `IAsyncEnumerable<T>` and `await foreach`

## Running the Examples

The `Program.Main` method calls all example types in sequence:

1. **Awaiter Pattern**: Demonstrates custom `TimeSpan` awaiting
2. **Async Enumeration**: Shows async foreach with custom collections
3. **Struct Enumeration**: Demonstrates zero-allocation enumeration
4. **Deconstruction**: Shows generated deconstruct methods in action

## Source Generator Details

The `DeconstructGenerator` analyzes classes marked with `[GeneratedDeconstruct]`:

- Examines each public constructor
- Maps constructor parameters to property assignments
- Generates `Deconstruct` methods with matching signatures
- Supports multiple constructors with different parameter counts
- Handles parameter name mapping to actual property assignments

## Build Requirements

- .NET 9.0
- C# 12.0
- Visual Studio 2022 or compatible IDE with C# 12 support

## Notes

- The async enumerator (C# 8.0) includes artificial delays to demonstrate async behavior
- Struct enumerators (C# 7.2) provide performance benefits by avoiding heap allocations
- The source generator (C# 9.0) respects constructor parameter order and assignment mapping
- Collection builders (C# 12.0) enable modern collection expression syntax
