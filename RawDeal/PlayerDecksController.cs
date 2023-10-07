using RawDealView.Formatters;

namespace RawDeal;

public class PlayerDecksController
{
    private int _handSize;
    private Dictionary<DeckName, List<IViewableCardInfo>> _playerDecks;
    
    public PlayerDecksController(Dictionary<DeckName, List<IViewableCardInfo>> playerDecks, int handSize)
    {
        _playerDecks = playerDecks;
        _handSize = handSize;
    }
    private Dictionary<DeckName, List<string>> BuildFormattedDecks()
    {
        var formattedDecks = new Dictionary<DeckName, List<string>>
        {
            { DeckName.Hand, FormatDeck(DeckName.Hand) },
            { DeckName.Ringside, FormatDeck(DeckName.Ringside) },
            { DeckName.RingArea, FormatDeck(DeckName.RingArea) }
        };
        return formattedDecks;
    }
    
    private List<string> FormatDeck(DeckName deckName)
    {
        return Utils.FormatDecksOfCards(_playerDecks[deckName]);
    }
    
    public Dictionary<DeckName, int> ShowUpdatedDeckCounts()
    {
        var deckInfo = new Dictionary<DeckName, int>
        {
            { DeckName.Hand, _playerDecks[DeckName.Hand].Count },
            { DeckName.Arsenal, _playerDecks[DeckName.Arsenal].Count }
        };
        return deckInfo;
    }

    public Dictionary<DeckName, List<IViewableCardInfo>> ShowUpdatedDeckInfo()
    {
        return _playerDecks;
    }

    public virtual void DrawCardsInTheBeginning()
    {
        Console.WriteLine(_handSize);
        for (int cardIndex = 0; cardIndex < _handSize; cardIndex++)
            DrawCard();
    }

    public virtual void DrawCard()
    {
        int lastCardOfTheArsenal = _playerDecks[DeckName.Arsenal].Count - 1;
        var arsenal = _playerDecks[DeckName.Arsenal];
        if (lastCardOfTheArsenal >= 0)
        {
            _playerDecks[DeckName.Hand].Add(arsenal[lastCardOfTheArsenal]);
            _playerDecks[DeckName.Arsenal].RemoveAt(lastCardOfTheArsenal);
        }
    }

    public void PassCardFromDeckToAnother(DeckName fromDeckName, DeckName toDeckName, int cardIndex)
    {
        var fromDeck = _playerDecks[fromDeckName];
        var toDeck = _playerDecks[toDeckName];
        var cardToPass = fromDeck[cardIndex];
        fromDeck.RemoveAt(cardIndex);
        toDeck.Add(cardToPass);
    }
    
    public void PassCardFromADeckToTheBackOfTheArsenal(DeckName fromDeckName, int cardIndex)
    {
        var fromDeck = _playerDecks[fromDeckName];
        var arsenal = _playerDecks[DeckName.Arsenal];
        var selectedCard = fromDeck[cardIndex]; 
        fromDeck.RemoveAt(cardIndex);
        arsenal.Insert(0, selectedCard);
    }
    public IViewableCardInfo PassCardFromArsenalToRingside()
    {
        var arsenalLength = _playerDecks[DeckName.Arsenal].Count;
        var selectedCard = _playerDecks[DeckName.Arsenal][arsenalLength - 1];
        _playerDecks[DeckName.Arsenal].RemoveAt(arsenalLength - 1);
        _playerDecks[DeckName.Ringside].Add(selectedCard);
        return selectedCard;
    }
}