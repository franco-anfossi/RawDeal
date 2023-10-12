using RawDeal.Data_Structures;
using RawDeal.Effects;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Cards;

public class BasicHybridController : BasicCardController
{
    private View _view;
    private IViewablePlayInfo _selectedPlay;
    private ImportantPlayerData _playerData;
    private ImportantPlayerData _opponentData;
    
    public BasicHybridController(
        ImportantPlayerData playerData, ImportantPlayerData opponentData, 
        IViewablePlayInfo selectedPlay, View view) : base(playerData, opponentData, selectedPlay, view)
    {
        _view = view;
        _selectedPlay = selectedPlay;
        _playerData = playerData;
        _opponentData = opponentData;
    }
    
    public override void ApplyEffect()
    {
        if (_selectedPlay.PlayedAs == "MANEUVER")
        {
            _playerData.DecksController.PassCardFromHandToRingArea(_selectedPlay.CardInfo);
            
            var playerMakeDamageEffect = new MakeDamageEffect(_playerData, _selectedPlay, _view);
            playerMakeDamageEffect.AddFortitudeToPlayer();
            
            var opponentMakeDamageEffect = new MakeDamageEffect(_opponentData, _selectedPlay, _view);
            opponentMakeDamageEffect.Apply();
        }
        else if (_selectedPlay.PlayedAs == "ACTION")
        {
            var cardToDiscard = _selectedPlay.CardInfo;
            
            var playerMustDiscardCardEffect = new MustDiscardHandCardEffect(_playerData, _view, cardToDiscard);
            playerMustDiscardCardEffect.Apply();
        }
    }
    
}