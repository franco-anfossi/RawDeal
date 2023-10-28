using System.Collections;
using System.Collections.Generic;

namespace RawDeal.Boundaries;

public class BoundaryList<T> : IEnumerable<T>
{
    private readonly List<T> _items;

    public BoundaryList()
    {
        _items = new List<T>();
    }

    public BoundaryList(IEnumerable<T> collection)
    {
        _items = new List<T>(collection);
    }
    
    public void Add(T item)
    {
        _items.Add(item);
    }

    public void Remove(T item)
    {
        _items.Remove(item);
    }

    public int RemoveAll(Predicate<T> match)
    {
        return _items.RemoveAll(match);
    }

    public void RemoveAt(int index)
    {
        _items.RemoveAt(index);
    }

    public void Insert(int index, T item)
    {
        _items.Insert(index, item);
    }

    public T this[int index]
    {
        get => _items[index];
        set => _items[index] = value;
    }

    public int Count => _items.Count;

    public List<T> ToList()
    {
        return new List<T>(_items);
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




