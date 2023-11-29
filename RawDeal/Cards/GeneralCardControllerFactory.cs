using RawDeal.Cards.Builders;
using RawDeal.Data_Structures;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Cards;

public class GeneralCardControllerFactory
{
    private readonly View _view;
    private readonly IViewablePlayInfo _selectedPlay;
    private readonly ImportantPlayerData _playerData;
    private readonly ImportantPlayerData _opponentData;
    private readonly LastCardUsed _lastCardUsed;

    public GeneralCardControllerFactory(ImportantPlayerData playerData, ImportantPlayerData opponentData, 
        IViewablePlayInfo selectedPlay, LastCardUsed lastCardUsed, View view)
    {
        _view = view;
        _playerData = playerData;
        _selectedPlay = selectedPlay;
        _opponentData = opponentData;
        _lastCardUsed = lastCardUsed;
    }
    
    public CardController BuildCardController()
    {
        var conditionBuilder = new ConditionBuilder(
            _playerData, _selectedPlay, _selectedPlay, _lastCardUsed);
        var conditions = conditionBuilder.BuildConditions();
        
        var effectBuilder = new GeneralEffectBuilder(_playerData, _opponentData, _selectedPlay, _view);
        var effects = effectBuilder.BuildEffects();

        return new CardController(effects, conditions);
    }
}