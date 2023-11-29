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

    public CardController Build(IViewablePlayInfo notFormattedSelectedPlay, LastCardUsed lastCardUsed)
    {
        var cardControllerFactory = new GeneralCardControllerFactory(_playerData, _opponentData, 
            notFormattedSelectedPlay, lastCardUsed, _view);
        return cardControllerFactory.BuildCardController();
    }
}
