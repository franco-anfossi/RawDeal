using RawDeal.Data_Structures;
using RawDeal.Decks;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Effects;

public class DiscardArsenalCardsEffect : Effect
{
    private int _cardsToDiscard;
    
    public DiscardArsenalCardsEffect(
        ImportantPlayerData superstarData, View view, int cardsToDiscard) : base(superstarData, view)
    {
        _view = view;
        _playerData = superstarData;
        _cardsToDiscard = cardsToDiscard;
    }

    public override void Apply()
    {
        _view.SayThatSuperstarWillTakeSomeDamage(_playerData.Name, _cardsToDiscard);
        for (int currentDamage = 1; currentDamage <= _cardsToDiscard; currentDamage++)
        {
            IViewableCardInfo selectedCard = _playerData.DecksController.PassCardFromArsenalToRingside();
            string formattedCardData = Formatter.CardToString(selectedCard);
            _view.ShowCardOverturnByTakingDamage(formattedCardData, currentDamage, _cardsToDiscard);
        }
    }
}