using RawDeal.Data_Structures;
using RawDeal.Exceptions;
using RawDeal.Reversals;
using RawDealView;
using RawDealView.Formatters;
using RawDealView.Options;

namespace RawDeal.Effects;

public class JockeyingForPositionEffect : Effect
{
    private readonly IViewablePlayInfo _selectedPlay;
    private readonly ImportantPlayerData _opponentData;
    
    public JockeyingForPositionEffect(ImportantPlayerData superstarData, ImportantPlayerData opponentData, 
        IViewablePlayInfo selectedPlay, View view) : base(superstarData, view)
    {
        _opponentData = opponentData;
        _selectedPlay = selectedPlay;
    }

    public override void Apply()
    {
        ApplyNonReversalEffect();
        ApplyJockeyForPositionEffect();
        HandleReversalCase();
    }

    private void ApplyNonReversalEffect()
    {
        if (_selectedPlay.PlayedAs != "REVERSAL")
        {
            TryToReverse();
            View.SayThatPlayerSuccessfullyPlayedACard();
            PlayerData.DecksController.PassCardFromHandToRingArea(_selectedPlay.CardInfo);
        }
    }
    
    private void TryToReverse()
    {
        var reversalFromHand = new ReversalFromHandController(PlayerData, _opponentData, _selectedPlay, View);
        reversalFromHand.SelectReversalFromHand();
    }

    private void ApplyJockeyForPositionEffect()
    {
        var selectedEffect = View.AskUserToSelectAnEffectForJockeyForPosition(PlayerData.Name);

        if (selectedEffect == SelectedEffect.NextGrappleIsPlus4D)
            PlayerData.ChangesByJockeyingForPosition.DamageAdded = 4;
        else
            _opponentData.ChangesByJockeyingForPosition.FortitudeNeeded = 8;
    }

    private void HandleReversalCase()
    {
        if (_selectedPlay.PlayedAs == "REVERSAL")
        {
            throw new EndOfTurnException();
        }
    }
}