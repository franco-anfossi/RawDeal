using RawDeal.Data_Structures;
using RawDeal.Decks;

namespace RawDeal.Superstars;

public class Mankind : Player
{
    public Mankind(SuperstarData superstarData) : base(superstarData)
    {
        SuperstarData = superstarData;
    }
    
    public override PlayerDecksController BuildPlayerDecksController()
    {
        PlayerDecksController = new MankindDecksController(DecksInfo, SuperstarData);
        return PlayerDecksController;
    }
    
    public override bool PlaySpecialAbility()
    {
        return true;
    }
    
    
}