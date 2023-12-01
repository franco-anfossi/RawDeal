using RawDeal.Boundaries;
using RawDeal.Data_Structures;
using RawDeal.Superstars;
using RawDealView.Formatters;

namespace RawDeal.Decks;

public class Deck
{
    private Player _playerDeckOwner;
    private readonly BoundaryList<IViewableCardInfo> _deckCards;
    private readonly CardsSet _cardsSet;
    
    public Player PlayerDeckOwner => _playerDeckOwner;
    public BoundaryList<IViewableCardInfo> DeckCards => _deckCards;
    
    public Deck(string[] openedDeckFromArchive, CardsSet cardsSet)
    {
        _deckCards = new BoundaryList<IViewableCardInfo>();
        _cardsSet = cardsSet;

        CreateDeck(openedDeckFromArchive);
    }

    private void CreateDeck(string[] openedDeckFromArchive)
    {
        AddEquivalentSuperstarToDeck(openedDeckFromArchive);
        AddEquivalentCardToDeck(openedDeckFromArchive);
        InitializeDeckSuperstarAttributes();
    }
    
    private void InitializeDeckSuperstarAttributes()
        => _playerDeckOwner.BuildDeckInfo(_deckCards);

    private void AddEquivalentSuperstarToDeck(string[] openDeckFromArchive)
    {
        string deckSuperstarName = openDeckFromArchive[0].Replace("(Superstar Card)", "").Trim();
        foreach (var superstar in _cardsSet.PossibleSuperstars)
        {
            if (superstar.CompareNames(deckSuperstarName))
                _playerDeckOwner = (Player)superstar.Clone();
        }
    }
    
    private void AddEquivalentCardToDeck(String[] openDeckFromArchive) 
    {
        foreach (var cardName in openDeckFromArchive)
            AddCardToDeckIfMatchFound(cardName);
    }

    private void AddCardToDeckIfMatchFound(String cardName) 
    {
        foreach (var card in _cardsSet.PossibleCards)
        {
            if (card.CompareCardTitle(cardName))
                AddCardToDeck(card);
        }
    }

    private void AddCardToDeck(CardData card) 
    {
        CardData cardDataCopy = CopyBaseCard(card);
        _deckCards.Add(cardDataCopy);
    }
    
    private CardData CopyBaseCard(CardData cardData)
    {
        CardData cardDataCopy = (CardData)cardData.Clone();
        return cardDataCopy;
    }
}