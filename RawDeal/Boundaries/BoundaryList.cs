using System.Collections;

namespace RawDeal.Boundaries;

public class BoundaryList<T> : IEnumerable<T>
{
    private readonly List<T> _items;

    public BoundaryList()
    {
        _items = new List<T>();
    }

    public void Add(T item)
    {
        _items.Add(item);
    }

    public void Remove(T item)
    {
        _items.Remove(item);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _items.GetEnumerator();
    }
}
