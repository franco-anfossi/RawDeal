using RawDeal.Boundaries;
using RawDeal.Data_Structures;
using RawDeal.Effects;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Cards.Builders;

public class ManeuverEffectBuilder : IEffectBuilder
{
    private readonly ImportantPlayerData _superstarData;
    private readonly ImportantPlayerData _opponentData;
    private readonly IViewablePlayInfo _selectedPlay;
    private readonly BoundaryList<Effect> _effects;
    private readonly View _view;
    
    public ManeuverEffectBuilder (ImportantPlayerData superstarData, ImportantPlayerData opponentData,
        IViewablePlayInfo selectedPlay, View view)
    {
        _view = view;
        _superstarData = superstarData;
        _opponentData = opponentData;
        _selectedPlay = selectedPlay;
        _effects = new BoundaryList<Effect>();
    }
    
    public BoundaryList<Effect> BuildEffects()
    {
        int numberOfCardsToMakeTheEffect;
        switch (_selectedPlay.CardInfo.Title)
        {
            case "Head Butt" or "Arm Drag" or "Arm Bar":
                numberOfCardsToMakeTheEffect = 1;
                AddGeneralInitialEffects();
                AddDiscardEffect(_superstarData, _superstarData, numberOfCardsToMakeTheEffect);
                AddDamageEffect();
                return _effects;
            
            case "Pump Handle Slam":
                numberOfCardsToMakeTheEffect = 2;
                AddGeneralInitialEffects();
                AddDiscardEffect(_opponentData, _opponentData, numberOfCardsToMakeTheEffect);
                AddDamageEffect();
                return _effects;
            
            case "Bear Hug" or "Choke Hold" or "Ankle Lock" or "Spinning Heel Kick" or "Figure Four Leg Lock" or 
                "Samoan Drop" or "Boston Crab" or "Power Slam" or "Torture Rack":
                numberOfCardsToMakeTheEffect = 1;
                AddGeneralInitialEffects();
                AddDiscardEffect(_opponentData, _opponentData, numberOfCardsToMakeTheEffect);
                AddDamageEffect();
                return _effects;
            
            case "Bulldog":
                numberOfCardsToMakeTheEffect = 1;
                AddGeneralInitialEffects();
                AddDiscardEffect(_superstarData, _superstarData, numberOfCardsToMakeTheEffect);
                AddDiscardEffect(_opponentData, _superstarData, numberOfCardsToMakeTheEffect);
                AddDamageEffect();
                return _effects;
            
            case "Headlock Takedown" or "Standing Side Headlock":
                numberOfCardsToMakeTheEffect = 1;
                AddGeneralInitialEffects();
                _effects.Add(new DrawCardsEffect(_opponentData, _view, numberOfCardsToMakeTheEffect));
                AddDamageEffect();
                return _effects;
            
            case "Kick" or "Running Elbow Smash":
                AddGeneralInitialEffects();
                _effects.Add(new MakeCollateralDamageEffect(_superstarData, _opponentData, _view));
                AddDamageEffect();
                return _effects;
            
            case "Double Leg Takedown" or "Reverse DDT":
                numberOfCardsToMakeTheEffect = 1;
                AddGeneralInitialEffects();
                _effects.Add(new AskToDrawEffect(_superstarData, _view, numberOfCardsToMakeTheEffect));
                AddDamageEffect();
                return _effects;
            
            case "Press Slam" or "DDT":
                numberOfCardsToMakeTheEffect = 2;
                AddGeneralInitialEffects();
                _effects.Add(new MakeCollateralDamageEffect(_superstarData, _opponentData, _view));
                AddDiscardEffect(_opponentData, _opponentData, numberOfCardsToMakeTheEffect);
                AddDamageEffect();
                return _effects;
            
            case "Fisherman's Suplex":
                numberOfCardsToMakeTheEffect = 1;
                AddGeneralInitialEffects();
                _effects.Add(new MakeCollateralDamageEffect(_superstarData, _opponentData, _view));
                _effects.Add(new AskToDrawEffect(_superstarData, _view, numberOfCardsToMakeTheEffect));
                AddDamageEffect();
                return _effects;
            
            case "Guillotine Stretch":
                numberOfCardsToMakeTheEffect = 1;
                AddGeneralInitialEffects();
                AddDiscardEffect(_opponentData, _opponentData, numberOfCardsToMakeTheEffect);
                _effects.Add(new AskToDrawEffect(_superstarData, _view, numberOfCardsToMakeTheEffect));
                AddDamageEffect();
                return _effects;
            
            case "Chicken Wing":
                numberOfCardsToMakeTheEffect = 2;
                AddGeneralInitialEffects();
                _effects.Add(new RecoverEffect(_superstarData, _view, numberOfCardsToMakeTheEffect));
                AddDamageEffect();
                return _effects;
                
            case "Lionsault":
                numberOfCardsToMakeTheEffect = 1;
                AddGeneralInitialEffects();
                AddDiscardEffect(_opponentData, _opponentData, numberOfCardsToMakeTheEffect);
                AddDamageEffect();
                return _effects;
            
            case "Tree of Woe":
                numberOfCardsToMakeTheEffect = 2;
                AddGeneralInitialEffects();
                AddDiscardEffect(_opponentData, _opponentData, numberOfCardsToMakeTheEffect);
                AddDamageEffect();
                return _effects;
            
            case "Austin Elbow Smash":
                AddGeneralInitialEffects();
                AddDamageEffect();
                return _effects;
            
            default:
                AddGeneralInitialEffects();
                AddDamageEffect();
                return _effects;
        }
    }

    private void AddGeneralInitialEffects()
    {
        _effects.Add(new ReverseFromHandEffect(_superstarData, _opponentData, _selectedPlay, _view));
        _effects.Add(new SuccessfullyPlayedCardEffect(_superstarData, _selectedPlay, _view));
    }
    
    private void AddDamageEffect()
        => _effects.Add(new MakeDamageEffect(_superstarData, _opponentData, _selectedPlay, _view));

    private void AddDiscardEffect(ImportantPlayerData superstarData, ImportantPlayerData deciderData, int numOfCards)
        => _effects.Add(new AskToDiscardHandCardsEffect(superstarData, deciderData, _view, numOfCards));
}