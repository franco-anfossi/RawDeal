using RawDeal.Data_Structures;
using RawDealView.Formatters;

namespace RawDeal.Conditions;

public class DamageOfLastCard : Condition
{
    private readonly int _damageToCompare;
    private readonly ImportantPlayerData _playerData;
    private readonly string _cardType;
    
    public DamageOfLastCard(ImportantPlayerData playerData, IViewablePlayInfo selectedPlay,
        int damageToCompare, string cardType) : base(selectedPlay)
    {
        _damageToCompare = damageToCompare;
        _playerData = playerData;
        _cardType = cardType;
    }
        
    public override bool Check()
    {
        var lastCardUsed = _playerData.LastCardUsed;
        int lastCardDamage = Convert.ToInt32(lastCardUsed.CardDamage);
        return CheckDamageOfLastCard(lastCardDamage) && CheckTypeOfLastCard(lastCardUsed.UsedType);
    }

    private bool CheckDamageOfLastCard(int lastCardDamage)
        => lastCardDamage >= _damageToCompare;
    
    private bool CheckTypeOfLastCard(string lastCardType)
        => lastCardType == _cardType;
}