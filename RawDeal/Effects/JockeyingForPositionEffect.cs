using RawDeal.Data_Structures;
using RawDeal.Exceptions;
using RawDeal.Reversals;
using RawDealView;
using RawDealView.Formatters;
using RawDealView.Options;

namespace RawDeal.Effects;

public class JockeyingForPositionEffect : Effect
{
    private readonly ImportantPlayerData _opponentData;
    
    public JockeyingForPositionEffect(ImportantPlayerData superstarData, 
        ImportantPlayerData opponentData, View view) : base(superstarData, view)
    {
        _opponentData = opponentData;
    }

    public override void Apply()
    {
        var selectedEffect = View.AskUserToSelectAnEffectForJockeyForPosition(PlayerData.Name);

        if (selectedEffect == SelectedEffect.NextGrappleIsPlus4D)
            PlayerData.ChangesByJockeyingForPosition.DamageAdded = 4;
        else
            _opponentData.ChangesByJockeyingForPosition.FortitudeNeeded = 8;
    }
}