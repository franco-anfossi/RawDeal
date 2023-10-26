using RawDealView;
using RawDealView.Formatters;
using RawDeal.Data_Structures;
using RawDeal.Effects;
using RawDeal.Reversals;

namespace RawDeal.Cards;

public class BasicCardController
{
    protected View View;
    protected IViewablePlayInfo SelectedPlay;
    protected ImportantPlayerData PlayerData;
    protected ImportantPlayerData OpponentData;
    
    public BasicCardController(ImportantPlayerData playerData, ImportantPlayerData opponentData, 
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