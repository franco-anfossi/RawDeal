using RawDealView.Formatters;

namespace RawDeal;

public class Deck
{
    private Player _playerDeckOwner;
    private List<IViewableCardInfo> _deckCards;
    private CardsSet _cardsSet;
    
    public Player PlayerDeckOwner => _playerDeckOwner;
    public List<IViewableCardInfo> DeckCards => _deckCards;
    
    public Deck(string[] openedDeckFromArchive, CardsSet cardsSet)
    {
        _deckCards = new List<IViewableCardInfo>();
        _cardsSet = cardsSet;

        AddEquivalentSuperstarToDeck(openedDeckFromArchive);
        AddEquivalentCardToDeck(openedDeckFromArchive);
        InitializeDeckSuperstarAttributes();
    }

    private void AddEquivalentSuperstarToDeck(string[] openDeckFromArchive)
    {
        string deckSuperstarName = openDeckFromArchive[0].Replace("(Superstar Card)", "").Trim();
        foreach (var superstar in _cardsSet.PossibleSuperstars)
        {
            if (superstar.CompareNames(deckSuperstarName))
            {
                _playerDeckOwner = (Player)superstar.Clone();
            }
        }
    }

    private void AddEquivalentCardToDeck(string[] openDeckFromArchive)
    {
        foreach (var cardName in openDeckFromArchive)
        foreach (var card in _cardsSet.PossibleCards)
        {
            if (cardName.Trim() == card.Title)
            {
                Card cardCopy = CopyBaseCard(card);
                _deckCards.Add(cardCopy);
            }
        }
    }
    private Card CopyBaseCard(Card card)
    {
        Card cardCopy = (Card)card.Clone();
        return cardCopy;
    }
    
    private void InitializeDeckSuperstarAttributes()
    {
        _playerDeckOwner.InitializeNecessaryAttributes(_deckCards);
    }
}