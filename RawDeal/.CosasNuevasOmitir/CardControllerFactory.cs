using RawDeal.Data_Structures;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.WNuevo;

public class CardControllerFactory
{
    private readonly View _view;
    private readonly IViewablePlayInfo _selectedPlay;
    private readonly ImportantPlayerData _playerData;
    private readonly ImportantPlayerData _opponentData;

    public CardControllerFactory(ImportantPlayerData playerData, ImportantPlayerData opponentData, 
        IViewablePlayInfo selectedPlay, View view)
    {
        _view = view;
        _playerData = playerData;
        _selectedPlay = selectedPlay;
        _opponentData = opponentData;
    }
}