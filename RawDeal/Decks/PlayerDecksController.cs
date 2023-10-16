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
    {
        return _playerDecks;
    }

    public List<IViewableCardInfo> SearchForReversalInHand(string reversalName)
    {
        var cardsWithName = new List<IViewableCardInfo>();
        foreach (var card in _playerDecks.Hand)
        {
            var hasReversal = card.Types.Contains("Reversal");
            var hasName = card.Subtypes.Contains(reversalName);
            if (hasReversal && hasName)
                cardsWithName.Add(card);
        }

        return cardsWithName;
    }

    public List<string> CreateFormattedPlays(List<IViewableCardInfo> cardsToPlays)
    {
        var formattedPlays = new List<string>();
        foreach (var card in cardsToPlays)
        {
            var play = new PlayInfo(card, card.Types[0].ToUpper());
            var formattedPlay = Formatter.PlayToString(play);
            formattedPlays.Add(formattedPlay);
        }

        return formattedPlays;
    }
    
    
    
    public (int, int) ShowUpdatedDeckCounts()
    {
        return (_playerDecks.Hand.Count, _playerDecks.Arsenal.Count);
    }
    
    public bool CheckForEmptyArsenal()
    {
        return _playerDecks.Arsenal.Count == 0;
    }

    public List<IViewableCardInfo> CheckForPlayableCards()
    {
        var playableCards = new List<IViewableCardInfo>();
        foreach (var card in _playerDecks.Hand)
        {
            var cardFortitude = Convert.ToInt32(card.Fortitude);
            if (cardFortitude <= _playerData.Fortitude)
                playableCards.Add(card);
        }

        return playableCards;
    }

    public virtual void DrawCardsInTheBeginning()
    {
        for (int cardIndex = 0; cardIndex < _playerData.HandSize; cardIndex++)
            DrawTurnCard();
    }

    public virtual void DrawTurnCard()
    {
        int lastCardOfTheArsenal = _playerDecks.Arsenal.Count - 1;
        var arsenal = _playerDecks.Arsenal;
        if (lastCardOfTheArsenal >= 0)
        {
            _playerDecks.Hand.Add(arsenal[lastCardOfTheArsenal]);
            _playerDecks.Arsenal.RemoveAt(lastCardOfTheArsenal);
        }
    }
    
    public void DrawCard()
    {
        int lastCardOfTheArsenal = _playerDecks.Arsenal.Count - 1;
        var arsenal = _playerDecks.Arsenal;
        if (lastCardOfTheArsenal >= 0)
        {
            _playerDecks.Hand.Add(arsenal[lastCardOfTheArsenal]);
            _playerDecks.Arsenal.RemoveAt(lastCardOfTheArsenal);
        }
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
        _playerDecks.Ringside.Add(selectedCard);
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