namespace RawDeal.Superstars;

public class Mankind : Player
{
    private bool _initialDraw = true;
    private int _lastArsenalCardIndex;
    
    public Mankind(SuperstarData superstarData) : base(superstarData)
    {
        Name = superstarData.Name;
        Logo = superstarData.Logo;
        HandSize = superstarData.HandSize;
        SuperstarValue = superstarData.SuperstarValue;
        SuperstarAbility = superstarData.SuperstarAbility;
    }
    
    public override PlayerDecksController BuildPlayerDecksController()
    {
        return new MankindDecksController(Decks, HandSize);
    }
    
    public override bool PlaySpecialAbility()
    {
        return true;
    }
    public override void DrawCardsInTheBeginning()
    {
        for (int i = 0; i < HandSize; i++)
            DrawCard();
        
        _initialDraw = false;
        UpdatePlayerInfo();
    }
    public override void DrawCard()
    {
        _lastArsenalCardIndex = Arsenal.Count - 1;
        if (VerifyConditionsToDrawTwoCards())
            DrawTwoCards();
        else
        {
            Hand.Add(Arsenal[_lastArsenalCardIndex]); 
            Arsenal.RemoveAt(_lastArsenalCardIndex);
        }

        UpdatePlayerInfo();
    }
    
    private bool VerifyConditionsToDrawTwoCards()
    {
        return _lastArsenalCardIndex >= 1 && Arsenal.Count >= 1 && !_initialDraw;
    }

    private void DrawTwoCards()
    {
        for (int iterator = 0; iterator < 2; iterator++)
        { 
            Hand.Add(Arsenal[_lastArsenalCardIndex - iterator]); 
            Arsenal.RemoveAt(_lastArsenalCardIndex - iterator); 
        }
    }
    
    
}