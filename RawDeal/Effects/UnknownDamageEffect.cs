using RawDeal.Data_Structures;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Effects;

public class UnknownDamageEffect : Effect
{
    private readonly IViewablePlayInfo _selectedPlay;
    
    public UnknownDamageEffect(ImportantPlayerData superstarData, IViewablePlayInfo selectedPlay, 
        View view) : base(superstarData, view)
    {
        _selectedPlay = selectedPlay;
    }

    public override void Apply()
    {
        _selectedPlay.CardInfo.Damage = "#";
    }
}