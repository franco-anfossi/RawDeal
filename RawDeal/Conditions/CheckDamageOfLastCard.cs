using RawDeal.Data_Structures;
using RawDealView.Formatters;

namespace RawDeal.Conditions;

public class CheckDamageOfLastCard : Condition
{
    private readonly LastCardUsed _lastCardUsed;
    private readonly int _damageToCompare;
    
    public CheckDamageOfLastCard(IViewablePlayInfo selectedPlay, 
        LastCardUsed lastCardUsed, int damageToCompare) : base(selectedPlay)
    {
        SelectedPlay = selectedPlay;
        _lastCardUsed = lastCardUsed;
        _damageToCompare = damageToCompare;
    }
        
    public override bool Check()
    {
        int cardDamage = Convert.ToInt32(_lastCardUsed.CardDamage);
        return cardDamage >= _damageToCompare && _lastCardUsed.UsedType == "MANEUVER";
    }

    
}