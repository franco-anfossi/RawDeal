using System.Collections;

namespace RawDeal.Boundaries;

public class StringCollection : IEnumerable<string>
{
    private readonly List<string> _strings;

    public StringCollection()
    {
        _strings = new List<string>();
    }

    public StringCollection(List<string> value)
    {
        _strings = value;
    }

    public void Add(string str)
    {
        _strings.Add(str);
    }

    public void Remove(string str)
    {
        _strings.Remove(str);
    }

    public IEnumerator<string> GetEnumerator()
    {
        return _strings.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _strings.GetEnumerator();
    }
}
