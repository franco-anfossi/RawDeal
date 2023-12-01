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
        // TODO: Encapsulate this in a factory
        var effects = new BoundaryList<Effect>();
        if (_selectedPlay.PlayedAs == "MANEUVER")
        {
            var effectBuilder = new ManeuverEffectBuilder(_playerData, _opponentData, _selectedPlay, _view);
            effects = effectBuilder.BuildEffects();
        }
        else if (_selectedPlay.PlayedAs == "ACTION")
        {
            var effectBuilder = new ActionEffectBuilder(_playerData, _opponentData, _selectedPlay, _view);
            effects = effectBuilder.BuildEffects();
        }

        return new CardController(effects, conditions);
    }
}