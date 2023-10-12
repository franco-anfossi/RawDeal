using RawDeal.Data_Structures;
using RawDeal.Decks;
using RawDealView;

namespace RawDeal.Effects;

public abstract class Effect
{
    protected View _view;
    protected ImportantPlayerData _playerData;
    
    protected Effect(ImportantPlayerData superstarData, View view)
    {
        _view = view;
        _playerData = superstarData;
    }
    
    public abstract void Apply();
}