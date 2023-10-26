using RawDeal.Data_Structures;
using RawDeal.Exceptions;
using RawDealView;
using RawDealView.Formatters;
using RawDealView.Options;

namespace RawDeal.Cards;

public class JockeyingForPosition : BasicCardController
{
    public JockeyingForPosition(ImportantPlayerData playerData, ImportantPlayerData opponentData, 
        IViewablePlayInfo selectedPlay, View view) : base(playerData, opponentData, selectedPlay, view)
    {
        View = view;
        SelectedPlay = selectedPlay;
        PlayerData = playerData;
        OpponentData = opponentData;
    }
    
    public override void ApplyEffect()
    {
        if (SelectedPlay.PlayedAs != "REVERSAL")
        {
            TryToReverse();
            View.SayThatPlayerSuccessfullyPlayedACard();
            PlayerData.DecksController.PassCardFromHandToRingArea(SelectedPlay.CardInfo);
        }
        var selectedEffect = View.AskUserToSelectAnEffectForJockeyForPosition(PlayerData.Name);
        if (selectedEffect == SelectedEffect.NextGrappleIsPlus4D)
        {
            PlayerData.ChangesByJockeyingForPosition.DamageAdded = 4;
        }
        else
            OpponentData.ChangesByJockeyingForPosition.FortitudeNeeded = 8;
        
        if (SelectedPlay.PlayedAs == "REVERSAL")
        {
            throw new EndOfTurnException();
        }
    }
}