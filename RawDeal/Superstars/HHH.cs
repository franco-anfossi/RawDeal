using RawDeal.Data_Structures;

namespace RawDeal.Superstars;

public class HHH : Player
{
    public HHH(SuperstarData superstarData) : base(superstarData)
    {
        SuperstarData = superstarData;
    }
    public override bool PlaySpecialAbility()
    {
        return true;
    }
}