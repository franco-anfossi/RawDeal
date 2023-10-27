using RawDeal.Data_Structures;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Cards;

public class CardControllerFactory
{
    private readonly View _view;
    private readonly IViewablePlayInfo _selectedPlay;
    private readonly ImportantPlayerData _playerData;
    private readonly ImportantPlayerData _opponentData;
    private readonly CardControllerTypes _cardControllerType;

    public CardControllerFactory(ImportantPlayerData playerData, ImportantPlayerData opponentData, 
        IViewablePlayInfo selectedPlay, CardControllerTypes cardControllerTypes, View view)
    {
        _view = view;
        _playerData = playerData;
        _selectedPlay = selectedPlay;
        _opponentData = opponentData;
        _cardControllerType = cardControllerTypes;
    }
    
    // TODO: Refactor this to make it functional with conditions, and effects for all cards.
    public CardController BuildCardController()
    {
        return _cardControllerType switch
        {
            CardControllerTypes.BasicHybridCard => new BasicHybridCardController(_playerData, _opponentData,
                _selectedPlay, _view),
            CardControllerTypes.PlayerDiscardCard => new CardPlayerDiscardsController(_playerData, _opponentData,
                _selectedPlay, _view),
            CardControllerTypes.JockeyingForPosition => new JockeyingForPosition(_playerData, _opponentData,
                _selectedPlay, _view),
            _ => new CardController(_playerData, _opponentData, _selectedPlay, _view)
        };
    }
}