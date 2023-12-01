using RawDeal.Data_Structures;
using RawDealView;

namespace RawDeal.Effects;

public class ResetIrishBonus : Effect
{
    private readonly ImportantPlayerData _opponentData;
    
    public ResetIrishBonus(ImportantPlayerData superstarData, 
        ImportantPlayerData opponentData, View view) : base(superstarData, view)
    {
        _opponentData = opponentData;
    }

    public override void Apply()
    {
        PlayerData.BonusSet.ChangesByIrishWhip.Reset();
        _opponentData.BonusSet.ChangesByIrishWhip.Reset();
    }
}