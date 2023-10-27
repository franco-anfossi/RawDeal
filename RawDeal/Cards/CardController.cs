using RawDealView;
using RawDealView.Formatters;
using RawDeal.Data_Structures;
using RawDeal.Effects;
using RawDeal.Reversals;

namespace RawDeal.Cards;

public class CardController
{
    protected readonly View View;
    protected readonly IViewablePlayInfo SelectedPlay;
    protected readonly ImportantPlayerData PlayerData;
    protected readonly ImportantPlayerData OpponentData;
    
    public CardController(ImportantPlayerData playerData, ImportantPlayerData opponentData, 
        IViewablePlayInfo selectedPlay, View view)
    {
        View = view;
        SelectedPlay = selectedPlay;
        PlayerData = playerData;
        OpponentData = opponentData;
    }
    
    public virtual void ApplyEffect()
    {
        TryToReverse();
        MakeDamage();
    }
    
    protected void TryToReverse()
    {
        var reversalFromHand = new ReversalFromHandController(PlayerData, OpponentData, SelectedPlay, View);
        reversalFromHand.SelectReversalFromHand();
    }
    
    protected void MakeDamage()
    {
        var makeDamageEffect = new MakeDamageEffect(PlayerData, OpponentData, SelectedPlay, View);
        makeDamageEffect.Apply();
    }
}