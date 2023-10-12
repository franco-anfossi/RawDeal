using RawDeal.Data_Structures;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Effects;

public class MustDiscardHandCardEffect : Effect
{
    private IViewableCardInfo _cardToDiscard;
    
    public MustDiscardHandCardEffect(
        ImportantPlayerData superstarData, View view, IViewableCardInfo cardToDiscard) : base(superstarData, view)
    {
        _view = view;
        _playerData = superstarData;
        _cardToDiscard = cardToDiscard;
    }
    
    public override void Apply()
    {
        _view.SayThatPlayerMustDiscardThisCard(_playerData.Name, _cardToDiscard.Title);
        _playerData.DecksController.PassCardFromHandToRingside(_cardToDiscard);
        _playerData.DecksController.DrawCard();
        _view.SayThatPlayerDrawCards(_playerData.Name, 1);
    }
}