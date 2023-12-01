using RawDeal.Data_Structures;
using RawDealView;

namespace RawDeal.Effects;

public class IrishWhipEffect : Effect
{
    public IrishWhipEffect(ImportantPlayerData superstarData, View view) : base(superstarData, view) { }

    public override void Apply()
        => PlayerData.BonusSet.ChangesByIrishWhip.DamageAdded = 5;
}