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
    
    private void DrawTwoCards()
    {
        for (int iterator = 0; iterator < 2; iterator++)
            PassCardFromArsenalToHand(_lastArsenalCardIndex--);
    }
    public override void DrawTurnCard()
    {
        _lastArsenalCardIndex = _playerDecks.Arsenal.Count - 1;
        if (VerifyConditionsToDrawTwoCards())
            DrawTwoCards();
        else
            PassCardFromArsenalToHand(_lastArsenalCardIndex);
    }
    
    private bool VerifyConditionsToDrawTwoCards()
        => _lastArsenalCardIndex >= 1 && _playerDecks.Arsenal.Count >= 1 && !_initialDraw;
}