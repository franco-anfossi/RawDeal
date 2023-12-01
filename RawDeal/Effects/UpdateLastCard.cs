using RawDeal.Data_Structures;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Effects;

public class UpdateLastCard : Effect
{
    private readonly IViewablePlayInfo _selectedCard;
    private readonly ImportantPlayerData _opponentData;
    
    public UpdateLastCard(ImportantPlayerData superstarData, ImportantPlayerData opponentData, 
        IViewablePlayInfo selectedCard, View view) : base(superstarData, view)
    {
        _opponentData = opponentData;
        _selectedCard = selectedCard;
    }

    public override void Apply()
    {
        IViewableCardInfo card = _selectedCard.CardInfo;
        PlayerData.LastCardUsed = new LastCardUsed(card.Damage, card.Fortitude, _selectedCard.PlayedAs);
        _opponentData.LastCardUsed = new LastCardUsed(card.Damage, card.Fortitude, _selectedCard.PlayedAs);
    }
}