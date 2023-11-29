namespace RawDeal.Bonus.Expiration_Conditions;

public class Never : ExpirationCondition
{
    public override bool CheckIfBonusIsExpired()
        => false; 
}