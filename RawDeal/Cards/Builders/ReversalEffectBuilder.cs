using RawDeal.Boundaries;
using RawDeal.Data_Structures;
using RawDeal.Effects;
using RawDeal.Reversals;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Cards.Builders;

public class ReversalEffectBuilder : IEffectBuilder
{
    private readonly ReversalPlayedFrom _reversalFrom;
    private readonly ImportantPlayerData _superstarData;
    private readonly ImportantPlayerData _opponentData;
    private readonly IViewablePlayInfo _selectedPlay;
    private readonly BoundaryList<Effect> _effects;
    private readonly View _view;
    
    public ReversalEffectBuilder(ImportantPlayerData superstarData, ImportantPlayerData opponentData,
        IViewablePlayInfo selectedPlay, ReversalPlayedFrom reversalFrom ,View view)
    {
        _view = view; 
        _superstarData = superstarData;
        _opponentData = opponentData;
        _selectedPlay = selectedPlay;
        _effects = new BoundaryList<Effect>();
        _reversalFrom = reversalFrom;
    }

    public BoundaryList<Effect> BuildEffects()
    {
        return _reversalFrom != ReversalPlayedFrom.PlayedFromArsenal ? 
            BuildReversalsFromHand() : BuildReversalFromArsenal();
    }

    private BoundaryList<Effect> BuildReversalsFromHand()
    {
        switch (_selectedPlay.CardInfo.Title)
        {
            case "Rolling Takedown" or "Knee to the Gut":
                _effects.Add(new MakeDamageEffect(_superstarData, _opponentData, _selectedPlay, _view));
                _effects.Add(new UnknownDamageEffect(_superstarData, _selectedPlay, _view));
                return BasicReversalEffect();
            case "Elbow to the Face":
                _effects.Add(new MakeDamageEffect(_superstarData, _opponentData, _selectedPlay, _view));
                return BasicReversalEffect();
            case "Manager Interferes":
                _effects.Add(new DrawCardsEffect(_superstarData, _view, 1));
                _effects.Add(new MakeDamageEffect(_superstarData, _opponentData, _selectedPlay, _view));
                return BasicReversalEffect();
            case "Chyna Interferes":
                _effects.Add(new DrawCardsEffect(_superstarData, _view, 2));
                _effects.Add(new MakeDamageEffect(_superstarData, _opponentData, _selectedPlay, _view));
                return BasicReversalEffect();
            case "Clean Break":
                _effects.Add(new AskToDiscardHandCardsEffect(
                    _opponentData, _opponentData, _view, 4));
                _effects.Add(new DrawCardsEffect(_superstarData, _view, 1));
                return BasicReversalEffect();
            case "Jockeying for Position":
                _effects.Add(new JockeyingForPositionEffect(_superstarData, _opponentData, _view));
                BasicReversalEffect();
                return _effects;
            
            default:
                return BasicReversalEffect();
        }
    }

    private BoundaryList<Effect> BuildReversalFromArsenal()
    {
        return BasicReversalEffect();
    }
    
    private BoundaryList<Effect> BasicReversalEffect()
    {
        _effects.Add(new BasicReversalEffect(_superstarData, _view));
        return _effects;
    }
}