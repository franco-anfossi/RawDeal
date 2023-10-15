using RawDeal.Data_Structures;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Effects;

public class MustDiscardHandCardEffect : Effect
{
    private readonly IViewableCardInfo _cardToDiscard;
    
    public MustDiscardHandCardEffect(
        ImportantPlayerData superstarData, View view, IViewableCardInfo cardToDiscard) : base(superstarData, view)
    {
        View = view;
        PlayerData = superstarData;
        _cardToDiscard = cardToDiscard;
    }
    
    public override void Apply()
    {
        View.SayThatPlayerMustDiscardThisCard(PlayerData.Name, _cardToDiscard.Title);
        PlayerData.DecksController.PassCardFromHandToRingside(_cardToDiscard);
        PlayerData.DecksController.DrawCard();
        View.SayThatPlayerDrawCards(PlayerData.Name, 1);
    }
}