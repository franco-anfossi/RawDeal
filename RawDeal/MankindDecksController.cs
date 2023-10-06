using RawDealView.Formatters;

namespace RawDeal;

public class MankindDecksController : PlayerDecksController
{
    private Dictionary<DeckName, List<IViewableCardInfo>> _playerDecks;
    private bool _initialDraw = true;
    private int _handSize;
    private int _lastArsenalCardIndex;
    
    public MankindDecksController(Dictionary<DeckName, List<IViewableCardInfo>> playerDecks, int handSize) : base(playerDecks, handSize)
    {
        _handSize = handSize;
        _playerDecks = playerDecks;
    }
    
    public override void DrawCardsInTheBeginning()
    {
        for (int i = 0; i < _handSize; i++)
            DrawCard();
        
        _initialDraw = false;
    }

    private void DrawTwoCards()
    {
        for (int iterator = 0; iterator < 2; iterator++)
        { 
            _playerDecks[DeckName.Hand].Add(_playerDecks[DeckName.Arsenal][_lastArsenalCardIndex - iterator]); 
            _playerDecks[DeckName.Arsenal].RemoveAt(_lastArsenalCardIndex - iterator); 
        }
    }
    public override void DrawCard()
    {
        _lastArsenalCardIndex = _playerDecks[DeckName.Arsenal].Count - 1;
        var arsenal = _playerDecks[DeckName.Arsenal];
        if (VerifyConditionsToDrawTwoCards())
            DrawTwoCards();
        else
        {
            _playerDecks[DeckName.Hand].Add(arsenal[_lastArsenalCardIndex]); 
            _playerDecks[DeckName.Arsenal].RemoveAt(_lastArsenalCardIndex);
        }
    }
    
    private bool VerifyConditionsToDrawTwoCards()
    {
        return _lastArsenalCardIndex >= 1 && _playerDecks[DeckName.Arsenal].Count >= 1 && !_initialDraw;
    }
    
    
}