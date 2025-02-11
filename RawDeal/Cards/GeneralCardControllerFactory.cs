using RawDeal.Boundaries;
using RawDeal.Cards.Builders;
using RawDeal.Data_Structures;
using RawDeal.Effects;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Cards;

public class GeneralCardControllerFactory
{
    private readonly View _view;
    private readonly IViewablePlayInfo _selectedPlay;
    private readonly ImportantPlayerData _playerData;
    private readonly ImportantPlayerData _opponentData;

    public GeneralCardControllerFactory(ImportantPlayerData playerData, ImportantPlayerData opponentData, 
        IViewablePlayInfo selectedPlay, View view)
    {
        _view = view;
        _playerData = playerData;
        _selectedPlay = selectedPlay;
        _opponentData = opponentData;
    }
    
    public CardController BuildCardController()
    {
        var conditionBuilder = new ConditionBuilder(_playerData, _selectedPlay, _selectedPlay);
        var conditions = conditionBuilder.BuildConditions();

        var effects = new BoundaryList<Effect>();
        if (_selectedPlay.PlayedAs == "MANEUVER")
            effects = ExecuteManeuverEffectBuilder();
        
        else if (_selectedPlay.PlayedAs == "ACTION")
            effects = ExecuteActionEffectBuilder();
        
        return new CardController(effects, conditions);
    }

    private BoundaryList<Effect> ExecuteManeuverEffectBuilder()
    {
        var effectBuilder = new ManeuverEffectBuilder(_playerData, _opponentData, _selectedPlay, _view);
        return effectBuilder.BuildEffects();
    }

    private BoundaryList<Effect> ExecuteActionEffectBuilder()
    {
        var effectBuilder = new ActionEffectBuilder(_playerData, _opponentData, _selectedPlay, _view);
        return effectBuilder.BuildEffects();
    }
}