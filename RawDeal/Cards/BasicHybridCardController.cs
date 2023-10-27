using RawDeal.Data_Structures;
using RawDeal.Effects;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Cards;

public class BasicHybridCardController : CardController
{
    public BasicHybridCardController(ImportantPlayerData playerData, ImportantPlayerData opponentData, 
        IViewablePlayInfo selectedPlay, View view) : base(playerData, opponentData, selectedPlay, view) { }
    
    public override void ApplyEffect()
    {
        if (SelectedPlay.PlayedAs == "MANEUVER")
        {
            ApplyManeuverEffect();
        }
        else if (SelectedPlay.PlayedAs == "ACTION")
        {
            ApplyActionEffect();
        }
    }

    private void ApplyManeuverEffect()
    {
        TryToReverse();
        MakeDamage();
    }

    private void ApplyActionEffect()
    {
        TryToReverse();
        var cardToDiscard = SelectedPlay.CardInfo;
        View.SayThatPlayerSuccessfullyPlayedACard();
        var playerMustDiscardCardEffect = new MustDiscardHandCardEffect(PlayerData, View, cardToDiscard);
        playerMustDiscardCardEffect.Apply();
    }
}