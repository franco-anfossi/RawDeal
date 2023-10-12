using RawDealView;
using RawDealView.Formatters;
using RawDeal.Data_Structures;
using RawDeal.Effects;

namespace RawDeal.Cards;

public class BasicCardController
{
    private View _view;
    private IViewablePlayInfo _selectedPlay;
    private ImportantPlayerData _playerData;
    private ImportantPlayerData _opponentData;
    
    public BasicCardController(ImportantPlayerData playerData, ImportantPlayerData opponentData, IViewablePlayInfo selectedPlay, View view)
    {
        _view = view;
        _selectedPlay = selectedPlay;
        _playerData = playerData;
        _opponentData = opponentData;
    }
    
    public virtual void ApplyEffect()
    {
        _playerData.DecksController.PassCardFromHandToRingArea(_selectedPlay.CardInfo);
        
        var playerMakeDamageEffect = new MakeDamageEffect(_playerData, _selectedPlay, _view);
        playerMakeDamageEffect.AddFortitudeToPlayer();
        
        var opponentMakeDamageEffect = new MakeDamageEffect(_opponentData, _selectedPlay, _view);
        opponentMakeDamageEffect.Apply();
    }
}