using RawDeal.Data_Structures;
using RawDealView;

namespace RawDeal.Effects;

public abstract class Effect
{
    protected View View;
    protected ImportantPlayerData PlayerData;
    
    protected Effect(ImportantPlayerData superstarData, View view)
    {
        View = view;
        PlayerData = superstarData;
    }

    public abstract void Apply();
}