namespace RawDeal.Superstars;

public class HHH : Player
{
    public HHH(SuperstarData superstarData) : base(superstarData)
    {
        Name = superstarData.Name;
        Logo = superstarData.Logo;
        HandSize = superstarData.HandSize;
        SuperstarValue = superstarData.SuperstarValue;
        SuperstarAbility = superstarData.SuperstarAbility;
    }
    public override bool PlaySpecialAbility()
    {
        return true;
    }
}