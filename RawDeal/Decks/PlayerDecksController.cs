using RawDeal.Boundaries;
using RawDeal.Data_Structures;
using RawDealView.Formatters;

namespace RawDeal.Decks;

public class PlayerDecksController
{
    private readonly DecksInfo _playerDecks;
    private readonly SuperstarData _playerData;
    
    public PlayerDecksController(DecksInfo playerDecks, SuperstarData superstarData)
    {
        _playerDecks = playerDecks;
        _playerData = superstarData;
    }
    public FormattedDecksInfo BuildFormattedDecks()
    {
        var formattedDecks = new FormattedDecksInfo(_playerDecks);
        return formattedDecks;
    }
    
    public DecksInfo BuildDecks()
        => _playerDecks;

    public BoundaryList<IViewableCardInfo> SearchForReversalInHand()
    {
        var reversalCards = new BoundaryList<IViewableCardInfo>();
        foreach (IViewableCardInfo card in _playerDecks.Hand)
            reversalCards = AddCardIfIsReversal(reversalCards, card);

        return reversalCards;
    }
    
    private BoundaryList<IViewableCardInfo> AddCardIfIsReversal(
        BoundaryList<IViewableCardInfo> reversalCards, IViewableCardInfo card)
    {
        var hasReversal = card.Types.Contains("Reversal");
        if (hasReversal)
            reversalCards.Add(card);
        
        return reversalCards;
    }
    
    public (int, int) ShowUpdatedDeckCounts()
        => (_playerDecks.Hand.Count, _playerDecks.Arsenal.Count);
    
    public bool CheckForEmptyArsenal()
        => _playerDecks.Arsenal.Count == 0;
    
    public bool CheckForEmptyHand()
        => _playerDecks.Hand.Count == 0;
    
    public bool CheckForEmptyRingside()
        => _playerDecks.Ringside.Count == 0;

    public BoundaryList<IViewableCardInfo> CheckForPlayableCards()
    {
        var playableCards = new BoundaryList<IViewableCardInfo>();
        foreach (IViewableCardInfo card in _playerDecks.Hand)
            playableCards.Add(card);

        return playableCards;
    }

    public virtual void DrawCardsInTheBeginning()
    {
        for (int cardIndex = 0; cardIndex < _playerData.HandSize; cardIndex++)
            DrawTurnCard();
    }

    public virtual void DrawTurnCard()
        => DrawCard();
    
    public void DrawCard()
    {
        int lastCardOfTheArsenal = _playerDecks.Arsenal.Count - 1;
        if (lastCardOfTheArsenal >= 0)
            PassCardFromArsenalToHand(lastCardOfTheArsenal);
    }

    protected void PassCardFromArsenalToHand(int lastCardOfTheArsenal)
    {
        _playerDecks.Hand.Add(_playerDecks.Arsenal[lastCardOfTheArsenal]);
        _playerDecks.Arsenal.RemoveAt(lastCardOfTheArsenal);
    }
    
    public void PassCardFromRingsideToTheBackOfTheArsenal(int cardIndex)
    {
        IViewableCardInfo selectedCard = _playerDecks.Ringside[cardIndex]; 
        _playerDecks.Ringside.RemoveAt(cardIndex);
        _playerDecks.Arsenal.Insert(0, selectedCard);
    }
    
    public void PassCardFromHandToTheBackOfTheArsenal(int cardIndex)
    {
        IViewableCardInfo selectedCard = _playerDecks.Hand[cardIndex]; 
        _playerDecks.Hand.RemoveAt(cardIndex);
        _playerDecks.Arsenal.Insert(0, selectedCard);
    }
    
    public IViewableCardInfo PassCardFromArsenalToRingside()
    {
        var arsenalLength = _playerDecks.Arsenal.Count;
        var selectedCard = _playerDecks.Arsenal[arsenalLength - 1];
        _playerDecks.Arsenal.RemoveAt(arsenalLength - 1);
        PassCardToRingside(selectedCard);
        return selectedCard;
    }
    
    public void PassCardToRingside(IViewableCardInfo selectedCard)
        => _playerDecks.Ringside.Add(selectedCard);

    public IViewableCardInfo DrawLastCardOfArsenal()
    {
        var arsenalLength = _playerDecks.Arsenal.Count;
        var selectedCard = _playerDecks.Arsenal[arsenalLength - 1];
        _playerDecks.Arsenal.RemoveAt(arsenalLength - 1);

        return selectedCard;
    }
    
    public void PassCardFromHandToRingArea(IViewableCardInfo selectedCard)
    {
        _playerDecks.RingArea.Add(selectedCard);
        _playerDecks.Hand.Remove(selectedCard);
    }
    
    public void PassCardFromHandToRingside(IViewableCardInfo selectedCard)
    {
        _playerDecks.Ringside.Add(selectedCard);
        _playerDecks.Hand.Remove(selectedCard);
    }

    public void PassCardFromRingsideToHand(int cardIndex)
    {
        IViewableCardInfo selectedCard = _playerDecks.Ringside[cardIndex]; 
        _playerDecks.Hand.Add(selectedCard);
        _playerDecks.Ringside.RemoveAt(cardIndex);
    }
}