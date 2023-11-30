using RawDeal.Cards;
using RawDeal.Data_Structures;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.OptionsPlayCard;

public class PlayExecutor
{
    private readonly View _view;

    public PlayExecutor(View view)
    {
        _view = view;
    }

    public LastCardUsed ExecutePlay(string playerName, 
        (IViewablePlayInfo, string) selectedPlay, CardController cardController)
    {
        var selectedPlayInfo = selectedPlay.Item1;
        var formattedSelectedPlay = selectedPlay.Item2;
        _view.SayThatPlayerIsTryingToPlayThisCard(playerName, formattedSelectedPlay);
        cardController.PlayCard();
        return new LastCardUsed(
            selectedPlayInfo.CardInfo.Damage, selectedPlayInfo.CardInfo.Fortitude, selectedPlayInfo.PlayedAs);
    }
}