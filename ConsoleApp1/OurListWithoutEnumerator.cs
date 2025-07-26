using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ConsoleApp1;

public static class OurListWithoutEnumerator
{
    public static OurListWithoutEnumerator<T> Create<T>(ReadOnlySpan<T> items) => new OurListWithoutEnumerator<T>(items);
}

[CollectionBuilder(typeof(OurListWithoutEnumerator), nameof(OurListWithoutEnumerator.Create))]
public class OurListWithoutEnumerator<T>
{ 
    private readonly List<T> _items = new();
    
    public OurListWithoutEnumerator(ReadOnlySpan<T> items)
    {
        _items.AddRange(items);
    }
    
    public OurListWithoutEnumerator(List<T> items)
    {
        _items=items;
    }

    public void Add(T item)
    {
        _items.Add(item);
    }
    
    public OurListWithoutEnumerator<T> Slice(int start, int length)
    {
        return new OurListWithoutEnumerator<T>(_items.Slice(start, length));
    }

    public void Add(T item, T item2)
    {
        _items.Add(item);
        _items.Add(item2);
    }
    public OurEnumerator GetEnumerator() => new OurEnumerator(_items);
    //IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
    public ref struct OurEnumerator // 12.0 : IDisposable
    {
        private readonly Span<T> _span;
        private int index = -1;

        public OurEnumerator(List<T> list)
        {
            _span = CollectionsMarshal.AsSpan(list);
        }
        public bool MoveNext()
        {
            if (++index < _span.Length)
            {
                return true;
            } 
            return false;
        }
        public ref T Current => ref _span[index];

        public void Dispose()
        {
            Console.WriteLine("dispose called");
        }
    }
    

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