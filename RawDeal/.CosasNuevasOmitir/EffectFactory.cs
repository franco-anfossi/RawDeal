using RawDeal.Data_Structures;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.WNuevo;

public class EffectFactory
{
    private readonly View _view;
    private readonly Conditions _conditions;
    private readonly IViewablePlayInfo _selectedPlay;
    private readonly ImportantPlayerData _playerData;
    private readonly ImportantPlayerData _opponentData;
    
    public EffectFactory(IViewablePlayInfo selectedPlay, ImportantPlayerData playerData, 
        ImportantPlayerData opponentData, Conditions conditions ,View view)
    {
        _view = view;
        _playerData = playerData;
        _opponentData = opponentData;
        _selectedPlay = selectedPlay;
        _conditions = conditions;
    }

    public void BuildEffect()
    {
        
    }
}