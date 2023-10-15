using RawDealView;
using RawDealView.Formatters;
using RawDeal.Data_Structures;
using RawDeal.Effects;

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
        PlayerData.DecksController.PassCardFromHandToRingArea(SelectedPlay.CardInfo);
        
        var makeDamageEffect = new MakeDamageEffect(PlayerData, OpponentData,SelectedPlay, View);
        makeDamageEffect.Apply();
    }
}