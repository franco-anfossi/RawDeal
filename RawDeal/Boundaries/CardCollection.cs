using System.Collections;
using RawDealView.Formatters;

namespace RawDeal.Boundaries;

public class CardCollection : IEnumerable<IViewableCardInfo>
{
    private readonly List<IViewableCardInfo> _cards;

    public CardCollection()
    {
        _cards = new List<IViewableCardInfo>();
    }

    public void Add(IViewableCardInfo card)
    {
        _cards.Add(card);
    }

    public void Remove(IViewableCardInfo card)
    {
        _cards.Remove(card);
    }

    public IEnumerator<IViewableCardInfo> GetEnumerator()
    {
        return _cards.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _cards.GetEnumerator();
    }
}
