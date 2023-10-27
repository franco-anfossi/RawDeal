using RawDeal.Cards;
using RawDeal.Data_Structures;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.OptionsPlayCard;

public class CardControllerBuilder
{
    private readonly ImportantPlayerData _playerData;
    private readonly ImportantPlayerData _opponentData;
    private readonly View _view;

    public CardControllerBuilder(ImportantPlayerData playerData, ImportantPlayerData opponentData, View view)
    {
        _playerData = playerData;
        _opponentData = opponentData;
        _view = view;
    }

    public CardController Build(IViewablePlayInfo notFormattedSelectedPlay)
    {
        var cardControllerType = DecideCardController(notFormattedSelectedPlay);
        var cardControllerFactory = new CardControllerFactory(_playerData, _opponentData, 
            notFormattedSelectedPlay, cardControllerType, _view);
        return cardControllerFactory.BuildCardController();
    }

    private CardControllerTypes DecideCardController(IViewablePlayInfo notFormattedSelectedPlay)
    {
        var cardControllerDecider = new CardControllerDecider(notFormattedSelectedPlay);
        return cardControllerDecider.DecideCardController();
    }
}
