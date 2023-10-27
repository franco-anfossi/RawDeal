using System.Collections;
using RawDealView.Formatters;

namespace RawDeal.Boundaries;

public class PlayCollection : IEnumerable<IViewablePlayInfo>
{
    private readonly List<IViewablePlayInfo> _plays;

    public PlayCollection()
    {
        _plays = new List<IViewablePlayInfo>();
    }

    public void Add(IViewablePlayInfo play)
    {
        _plays.Add(play);
    }

    public void Remove(IViewablePlayInfo play)
    {
        _plays.Remove(play);
    }

    public IEnumerator<IViewablePlayInfo> GetEnumerator()
    {
        return _plays.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _plays.GetEnumerator();
    }
}
