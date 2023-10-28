using RawDeal.Data_Structures;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Effects;

public class BasicHybridEffect : Effect
{
    private readonly ImportantPlayerData _opponentData;
    private readonly IViewablePlayInfo _selectedPlay;

    public BasicHybridEffect(ImportantPlayerData superstarData, ImportantPlayerData opponentData,
        IViewablePlayInfo selectedPlay, View view) : base(superstarData, view)
    {
        _opponentData = opponentData;
        _selectedPlay = selectedPlay;
    }
    
    public override void Apply()
    {
        if (_selectedPlay.PlayedAs == "MANEUVER")
        {
            ApplyManeuverEffect();
        }
        else if (_selectedPlay.PlayedAs == "ACTION")
        {
            ApplyActionEffect();
        }
    }

    private void ApplyManeuverEffect()
    {
        var successfullyPlayedCardEffect = new SuccessfullyPlayedCardEffect(PlayerData, _selectedPlay, View);
        successfullyPlayedCardEffect.Apply();
        
        var makeDamageEffect = new MakeDamageEffect(PlayerData, _opponentData, _selectedPlay, View);
        makeDamageEffect.Apply();
    }

    private void ApplyActionEffect()
    {
        View.SayThatPlayerSuccessfullyPlayedACard();
        var cardToDiscard = _selectedPlay.CardInfo;
        var playerMustDiscardCardEffect = new MustDiscardHandCardEffect(PlayerData, View, cardToDiscard);
        playerMustDiscardCardEffect.Apply();
    }
}