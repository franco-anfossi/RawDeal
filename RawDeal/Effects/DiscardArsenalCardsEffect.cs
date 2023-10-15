using RawDeal.Data_Structures;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Effects;

public class DiscardArsenalCardsEffect : Effect
{
    private readonly int _cardsToDiscard;
    
    public DiscardArsenalCardsEffect(
        ImportantPlayerData superstarData, View view, int cardsToDiscard) : base(superstarData, view)
    {
        View = view;
        PlayerData = superstarData;
        _cardsToDiscard = cardsToDiscard;
    }

    public override void Apply()
    {
        View.SayThatSuperstarWillTakeSomeDamage(PlayerData.Name, _cardsToDiscard);
        for (int currentDamage = 1; currentDamage <= _cardsToDiscard; currentDamage++)
        {
            IViewableCardInfo selectedCard = PlayerData.DecksController.PassCardFromArsenalToRingside();
            string formattedCardData = Formatter.CardToString(selectedCard);
            View.ShowCardOverturnByTakingDamage(formattedCardData, currentDamage, _cardsToDiscard);
        }
    }
}