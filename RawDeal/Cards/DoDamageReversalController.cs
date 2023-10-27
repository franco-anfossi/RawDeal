using RawDeal.Data_Structures;
using RawDeal.Exceptions;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Cards;

public class DoDamageReversalController : CardController
{
    public DoDamageReversalController(ImportantPlayerData playerData, ImportantPlayerData opponentData, 
        IViewablePlayInfo selectedPlay, View view) : base(playerData, opponentData, selectedPlay, view) { }
    
    public override void ApplyEffect()
    {
        MakeDamage();
        ChangeDamageToUnknownWhenNecessary();
        throw new EndOfTurnException();
    }

    private void ChangeDamageToUnknownWhenNecessary()
    {
        if (SelectedPlay.CardInfo.Damage[0] == '#')
            SelectedPlay.CardInfo.Damage = "#";
    }

    
}