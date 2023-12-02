using System.Collections;
using System.Collections.Generic;
using RawDeal.Exceptions;

namespace RawDeal.Boundaries;

public class BoundaryDict<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>> where TKey : notnull
{
    private readonly Dictionary<TKey, TValue> _dictionary;
    
    public Dictionary<TKey, TValue> ToDictionary()
        => new(_dictionary);

    public BoundaryDict(Dictionary<TKey, TValue> dictionary)
        => _dictionary = dictionary;
    
    public TValue this[TKey key]
    {
        get => _dictionary[key];
        set => _dictionary[key] = value;
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        => throw new DictionaryException();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
