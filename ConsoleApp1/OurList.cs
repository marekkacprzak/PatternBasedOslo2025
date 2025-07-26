using System.Collections;

namespace ConsoleApp1;

public class OurList<T> : IEnumerable<T>
{ 
    private readonly List<T> _items = new();
    public void Add(T item)
    {
        _items.Add(item);
    }

    public void Add(T item, T item2)
    {
        _items.Add(item);
        _items.Add(item2);
    }
    public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}