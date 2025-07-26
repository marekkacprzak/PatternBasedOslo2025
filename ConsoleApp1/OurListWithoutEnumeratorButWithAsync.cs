using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ConsoleApp1;


public static class OurListWithoutEnumeratorButWithAsync
{
    public static OurListWithoutEnumeratorButWithAsync<T> Create<T>(ReadOnlySpan<T> items) => new OurListWithoutEnumeratorButWithAsync<T>(items);
}

[CollectionBuilder(typeof(OurListWithoutEnumeratorButWithAsync), nameof(OurListWithoutEnumeratorButWithAsync.Create))]
public class OurListWithoutEnumeratorButWithAsync<T>
{ 
    private readonly List<T> _items = new();
    
    public OurListWithoutEnumeratorButWithAsync(ReadOnlySpan<T> items)
    {
        _items.AddRange(items);
    }
    
    public OurListWithoutEnumeratorButWithAsync(List<T> items)
    {
        _items=items;
    }

    public void Add(T item)
    {
        _items.Add(item);
    }
    
    public OurListWithoutEnumeratorButWithAsync<T> Slice(int start, int length)
    {
        return new OurListWithoutEnumeratorButWithAsync<T>(_items.Slice(start, length));
    }

    public void Add(T item, T item2)
    {
        _items.Add(item);
        _items.Add(item2);
    }
    public OurEnumerator GetEnumerator() => new(_items);
    //IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
    public OurEnumeratorWithAsync GetAsyncEnumerator() => new(_items);

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

    public class OurEnumeratorWithAsync //: IAsyncDisposable // 12.0 : IDisposable
    {
        private readonly T[] _span;
        private int index = -1;

        public OurEnumeratorWithAsync(List<T> list)
        {
            _span = list.ToArray();
        }
        public async ValueTask<bool> MoveNextAsync()
        {
            await Task.Delay(500);
            if (++index < _span.Length)
            {
                return true;
            } 
            return false;
        }
        public T Current => _span[index];

        public ValueTask DisposeAsync()
        {
            Console.WriteLine("async dispose called");
            return default;
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