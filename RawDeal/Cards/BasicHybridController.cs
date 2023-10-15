using RawDeal.Data_Structures;
using RawDeal.Effects;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Cards;

public class BasicHybridController : BasicCardController
{
    public BasicHybridController(ImportantPlayerData playerData, ImportantPlayerData opponentData, 
        IViewablePlayInfo selectedPlay, View view) : base(playerData, opponentData, selectedPlay, view)
    {
        View = view;
        SelectedPlay = selectedPlay;
        PlayerData = playerData;
        OpponentData = opponentData;
    }
    
    public override void ApplyEffect()
    {
        if (SelectedPlay.PlayedAs == "MANEUVER")
        {
            PlayerData.DecksController.PassCardFromHandToRingArea(SelectedPlay.CardInfo);
            
            var makeDamageEffect = new MakeDamageEffect(PlayerData, OpponentData,SelectedPlay, View);
            makeDamageEffect.Apply();
        }
        else if (SelectedPlay.PlayedAs == "ACTION")
        {
            var cardToDiscard = SelectedPlay.CardInfo;
            
            var playerMustDiscardCardEffect = new MustDiscardHandCardEffect(PlayerData, View, cardToDiscard);
            playerMustDiscardCardEffect.Apply();
        }
    }
    
}