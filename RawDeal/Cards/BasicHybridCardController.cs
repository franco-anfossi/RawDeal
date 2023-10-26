using RawDeal.Data_Structures;
using RawDeal.Effects;
using RawDeal.Reversals;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Cards;

public class BasicHybridCardController : BasicCardController
{
    public BasicHybridCardController(ImportantPlayerData playerData, ImportantPlayerData opponentData, 
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
            TryToReverse();
            MakeDamage();
        }
        else if (SelectedPlay.PlayedAs == "ACTION")
        {
            var cardToDiscard = SelectedPlay.CardInfo;
            TryToReverse();
            View.SayThatPlayerSuccessfullyPlayedACard();
            var playerMustDiscardCardEffect = new MustDiscardHandCardEffect(PlayerData, View, cardToDiscard);
            playerMustDiscardCardEffect.Apply();
        }
    }
    
}