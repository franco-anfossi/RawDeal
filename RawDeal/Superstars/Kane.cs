using RawDeal.Data_Structures;
using RawDeal.Effects;

namespace RawDeal.Superstars;

public class Kane : Player
{
    private const int CardsToDiscard = 1;
    
    public Kane(SuperstarData superstarData) : base(superstarData) { }
    
    public override void PlaySpecialAbility()
    {
        View.SayThatPlayerIsGoingToUseHisAbility(SuperstarData.Name, SuperstarData.SuperstarAbility);
        var opponentDiscardArsenalCardsEffect = new DiscardArsenalCardsEffect(OpponentData, View, CardsToDiscard);
        
        if (!OpponentData.DecksController.CheckForEmptyArsenal())
            opponentDiscardArsenalCardsEffect.Apply();
    }
}