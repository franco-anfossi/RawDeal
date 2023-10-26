using RawDeal.Data_Structures;
using RawDeal.Effects;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Nuevo;

public class EffectFactory
{
    private View _view;
    private ImportantPlayerData _playerData;
    private ImportantPlayerData _opponentData;
    private IViewablePlayInfo _selectedPlay;
    private List<Effect> _effects;
    
    public EffectFactory(ImportantPlayerData playerData, ImportantPlayerData opponentData, 
        IViewablePlayInfo selectedPlay, View view)
    {
        _view = view;
        _playerData = playerData;
        _opponentData = opponentData;
        _selectedPlay = selectedPlay;
    }
    
    public List<Effect> BuildEffects()
    {
        return _selectedPlay.CardInfo.Title switch
        {
            "Chop" => new List<Effect>
            {
                new MakeDamageEffect(_playerData, _opponentData, _selectedPlay, _view)
            },
            "Arm Bar Takedown" => new List<Effect>
            {
                new MakeDamageEffect(_playerData, _opponentData, _selectedPlay, _view)
            },
            "Collar & Elbow Lockup" => new List<Effect>
            {
                new MakeDamageEffect(_playerData, _opponentData, _selectedPlay, _view)
            },
            "Step Aside" => new List<Effect>
            {
                new MakeDamageEffect(_playerData, _opponentData, _selectedPlay, _view)
            },
            "Escape Move" => new List<Effect>
            {
                new MakeDamageEffect(_playerData, _opponentData, _selectedPlay, _view)
            },
            "Break the Hold" => new List<Effect>
            {
                new MakeDamageEffect(_playerData, _opponentData, _selectedPlay, _view)
            },
            "No Chance in Hell" => new List<Effect>
            {
                new MakeDamageEffect(_playerData, _opponentData, _selectedPlay, _view)
            },
            "Rolling Takedown" => new List<Effect>
            {
                new MakeDamageEffect(_playerData, _opponentData, _selectedPlay, _view)
            },
            "Knee to the Gut" => new List<Effect>
            {
                new MakeDamageEffect(_playerData, _opponentData, _selectedPlay, _view)
            },
            "Elbow to the Face" => new List<Effect>
            {
                new MakeDamageEffect(_playerData, _opponentData, _selectedPlay, _view)
            },
            "Manager Interferes" => new List<Effect>
            {
                new MakeDamageEffect(_playerData, _opponentData, _selectedPlay, _view)
            },
            "Chyna Interferes" => new List<Effect>
            {
                new MakeDamageEffect(_playerData, _opponentData, _selectedPlay, _view)
            },
            "Clean Break" => new List<Effect>
            {
                new MakeDamageEffect(_playerData, _opponentData, _selectedPlay, _view)
            },
            "Jockeying for Position" => new List<Effect>
            {
                new MakeDamageEffect(_playerData, _opponentData, _selectedPlay, _view)
            },
            _ => throw new ArgumentOutOfRangeException()
        };

    }
}