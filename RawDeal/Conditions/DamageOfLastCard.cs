using RawDeal.Data_Structures;
using RawDealView.Formatters;

namespace RawDeal.Conditions;

public class DamageOfLastCard : Condition
{
    private readonly int _damageToCompare;
    private readonly ImportantPlayerData _playerData;
    
    public DamageOfLastCard(ImportantPlayerData playerData, IViewablePlayInfo selectedPlay,
        int damageToCompare) : base(selectedPlay)
    {
        _damageToCompare = damageToCompare;
        _playerData = playerData;
    }
        
    public override bool Check()
    {
        var lastCardUsed = _playerData.LastCardUsed;
        int cardDamage = Convert.ToInt32(lastCardUsed.CardDamage);
        return cardDamage >= _damageToCompare && lastCardUsed.UsedType == "MANEUVER";
    }
}