using RawDeal.Cards;
using RawDeal.Data_Structures;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.OptionsPlayCard;

public class PlayExecutor
{
    private readonly View _view;

    public PlayExecutor(View view)
        => _view = view;

    public void ExecutePlay(string playerName, string formattedSelectedPlay, CardController cardController)
    {
        _view.SayThatPlayerIsTryingToPlayThisCard(playerName, formattedSelectedPlay);
        cardController.PlayCard();
    }
}