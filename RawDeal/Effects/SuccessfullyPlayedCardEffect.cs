using RawDeal.Data_Structures;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Effects;

public class SuccessfullyPlayedCardEffect : Effect
{
    private readonly IViewablePlayInfo _selectedPlay;

    public SuccessfullyPlayedCardEffect(ImportantPlayerData superstarData,
        IViewablePlayInfo selectedPlay, View view) : base(superstarData, view)
    {
        _selectedPlay = selectedPlay;
    }

    public override void Apply()
    {
        View.SayThatPlayerSuccessfullyPlayedACard();
        PlayerData.DecksController.PassCardFromHandToRingArea(_selectedPlay.CardInfo);
    }
}