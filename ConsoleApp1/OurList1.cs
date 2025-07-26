using System.Collections;
using System.Runtime.CompilerServices;

namespace ConsoleApp1;

public static class OurListWithSpan
{
    public static OurListWithSpan<T> Create<T>(ReadOnlySpan<T> items) => new OurListWithSpan<T>(items);
}

[CollectionBuilder(typeof(OurListWithSpan), nameof(OurListWithSpan.Create))]
public class OurListWithSpan<T> : IEnumerable<T>
{ 
    private readonly List<T> _items = new();

    public OurListWithSpan(ReadOnlySpan<T> items)
    {
        _items.AddRange(items);
    }
    
    public OurListWithSpan(List<T> items)
    {
        _items=items;
    }

    public void Add(T item)
    {
        _items.Add(item);
    }
    
    public OurListWithSpan<T> Slice(int start, int length)
    {
        return new OurListWithSpan<T>(_items.Slice(start, length));
    }

    public void Add(T item, T item2)
    {
        _items.Add(item);
        _items.Add(item2);
    }
    public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // explicit indexer for the collection
    // public string this[Index index]
    // {
    //     get => this[index];
    // }
    
    //implementing implicit indexer for the collection
    public T this[int index]  => _items[index]; 
    
    //for span support
    public int Length => _items.Count;
    //collection that grow and shring that can grow - count
    public int Count => _items.Count;
}