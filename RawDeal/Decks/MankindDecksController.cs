using RawDeal.Data_Structures;

namespace RawDeal.Decks;

public class MankindDecksController : PlayerDecksController
{
    private readonly DecksInfo _playerDecks;
    private bool _initialDraw = true;
    private readonly SuperstarData _playerData;
    private int _lastArsenalCardIndex;
    
    
    public MankindDecksController(DecksInfo playerDecks, SuperstarData superstarData) : base(playerDecks, superstarData)
    {
        _playerDecks = playerDecks;
        _playerData = superstarData;
    }
    
    public override void DrawCardsInTheBeginning()
    {
        for (int i = 0; i < _playerData.HandSize; i++)
            DrawTurnCard();
        
        _initialDraw = false;
    }
    
    // TODO: Refactor this 2 methods because duplicated code
    private void DrawTwoCards()
    {
        for (int iterator = 0; iterator < 2; iterator++)
        { 
            _playerDecks.Hand.Add(_playerDecks.Arsenal[_lastArsenalCardIndex - iterator]); 
            _playerDecks.Arsenal.RemoveAt(_lastArsenalCardIndex - iterator); 
        }
    }
    public override void DrawTurnCard()
    {
        _lastArsenalCardIndex = _playerDecks.Arsenal.Count - 1;
        var arsenal = _playerDecks.Arsenal;
        if (VerifyConditionsToDrawTwoCards())
            DrawTwoCards();
        else
        {
            _playerDecks.Hand.Add(arsenal[_lastArsenalCardIndex]); 
            _playerDecks.Arsenal.RemoveAt(_lastArsenalCardIndex);
        }
    }
    
    private bool VerifyConditionsToDrawTwoCards()
        => _lastArsenalCardIndex >= 1 && _playerDecks.Arsenal.Count >= 1 && !_initialDraw;
}