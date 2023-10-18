using RawDeal.Data_Structures;
using RawDeal.Effects;
using RawDeal.Reversals;
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
            var makeDamageEffect = new MakeDamageEffect(PlayerData, OpponentData,SelectedPlay, View);
            makeDamageEffect.Apply();
        }
        else if (SelectedPlay.PlayedAs == "ACTION")
        {
            
            var cardToDiscard = SelectedPlay.CardInfo;
            
            var reversalFromHand = new ReversalFromHandController(PlayerData, OpponentData, SelectedPlay, View);
            reversalFromHand.SelectReversalFromHand();
            
            View.SayThatPlayerSuccessfullyPlayedACard();
            var playerMustDiscardCardEffect = new MustDiscardHandCardEffect(PlayerData, View, cardToDiscard);
            playerMustDiscardCardEffect.Apply();
        }
    }
    
}