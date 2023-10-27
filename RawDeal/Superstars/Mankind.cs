using RawDeal.Data_Structures;
using RawDeal.Decks;

namespace RawDeal.Superstars;

public class Mankind : Player
{
    public Mankind(SuperstarData superstarData) : base(superstarData) { }

    protected override void BuildPlayerDecksController()
    {
        PlayerDecksController = new MankindDecksController(DecksInfo, SuperstarData);
    }
}