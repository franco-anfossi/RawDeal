using RawDeal.Boundaries;
using RawDeal.Data_Structures;
using RawDeal.Effects;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Cards.Builders;

public class GeneralEffectBuilder : IEffectBuilder
{
    private readonly ImportantPlayerData _superstarData;
    private readonly ImportantPlayerData _opponentData;
    private readonly IViewablePlayInfo _selectedPlay;
    private readonly BoundaryList<Effect> _effects;
    private readonly View _view;
    
    public GeneralEffectBuilder (ImportantPlayerData superstarData, ImportantPlayerData opponentData,
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
        // TODO: CHANGE DIRECT NUMBERS TO VARIABLES
        switch (_selectedPlay.CardInfo.Title)
        {
            case "Chop" or "Arm Bar Takedown" or "Collar & Elbow Lockup":
                // TODO: Probably necessary to add condition
                _effects.Add(new ReverseFromHandEffect(_superstarData, _opponentData, _selectedPlay, _view));
                _effects.Add(new BasicHybridEffect(_superstarData, _opponentData, _selectedPlay, _view));
                return _effects;
            
            case "Jockeying for Position":
                AddGeneralInitialEffects();
                _effects.Add(new JockeyingForPositionEffect(_superstarData, _opponentData, _view));
                return _effects;
            
            case "Head Butt" or "Arm Drag" or "Arm Bar":
                AddGeneralInitialEffects();
                AddDiscardEffect(_superstarData, _superstarData, 1);
                AddDamageEffect();
                return _effects;
            
            case "Pump Handle Slam":
                AddGeneralInitialEffects();
                AddDiscardEffect(_opponentData, _opponentData, 2);
                AddDamageEffect();
                return _effects;
            
            case "Bear Hug" or "Choke Hold" or "Ankle Lock" or "Spinning Heel Kick" or "Figure Four Leg Lock" or 
                "Samoan Drop" or "Boston Crab" or "Power Slam" or "Torture Rack":
                AddGeneralInitialEffects();
                AddDiscardEffect(_opponentData, _opponentData, 1);
                AddDamageEffect();
                return _effects;
            
            case "Bulldog":
                AddGeneralInitialEffects();
                AddDiscardEffect(_superstarData, _superstarData, 1);
                AddDiscardEffect(_opponentData, _superstarData, 1);
                AddDamageEffect();
                return _effects;
            
            case "Headlock Takedown" or "Standing Side Headlock":
                AddGeneralInitialEffects();
                _effects.Add(new DrawCardsEffect(_opponentData, _view, 1));
                AddDamageEffect();
                return _effects;
            
            case "Kick" or "Running Elbow Smash":
                AddGeneralInitialEffects();
                _effects.Add(new MakeCollateralDamageEffect(_superstarData, _opponentData, _view));
                AddDamageEffect();
                return _effects;
            
            case "Double Leg Takedown" or "Reverse DDT":
                AddGeneralInitialEffects();
                _effects.Add(new AskToDrawEffect(_superstarData, _view, 1));
                AddDamageEffect();
                return _effects;
            
            case "Undertaker’s Tombstone Piledriver":
                // TODO: Probably necessary to add condition
                _effects.Add(new ReverseFromHandEffect(_superstarData, _opponentData, _selectedPlay, _view));
                // TODO: Correct this part to be more general and more applicable to this card with "conditions"
                _effects.Add(new BasicHybridEffect(_superstarData, _opponentData, _selectedPlay, _view));
                return _effects;
            
            case "Offer Handshake":
                AddGeneralInitialEffects();
                _effects.Add(new AskToDrawEffect(_superstarData, _view, 3));
                AddDiscardEffect(_superstarData, _superstarData, 1);
                AddDamageEffect();
                return _effects;
            
            case "Press Slam" or "DDT":
                AddGeneralInitialEffects();
                _effects.Add(new MakeCollateralDamageEffect(_superstarData, _opponentData, _view));
                AddDiscardEffect(_opponentData, _opponentData, 2);
                AddDamageEffect();
                return _effects;
            
            case "Fisherman's Suplex":
                AddGeneralInitialEffects();
                _effects.Add(new MakeCollateralDamageEffect(_superstarData, _opponentData, _view));
                _effects.Add(new AskToDrawEffect(_superstarData, _view, 1));
                AddDamageEffect();
                return _effects;
            
            case "Guillotine Stretch":
                AddGeneralInitialEffects();
                AddDiscardEffect(_opponentData, _opponentData, 1);
                _effects.Add(new AskToDrawEffect(_superstarData, _view, 1));
                AddDamageEffect();
                return _effects;
            
            case "Spit At Opponent":
                AddGeneralInitialEffects();
                AddDiscardEffect(_superstarData, _superstarData, 1);
                AddDiscardEffect(_opponentData, _opponentData, 4);
                AddDamageEffect();
                return _effects;
            
            case "Chicken Wing":
                AddGeneralInitialEffects();
                _effects.Add(new RecoverEffect(_superstarData, _view, 2));
                AddDamageEffect();
                return _effects;
            
            case "Puppies! Puppies!":
                AddGeneralInitialEffects();
                _effects.Add(new RecoverEffect(_superstarData, _view, 5));
                _effects.Add(new DrawCardsEffect(_superstarData, _view, 2));
                AddDamageEffect();
                return _effects;
            
            case  "Recovery":
                AddGeneralInitialEffects();
                _effects.Add(new RecoverEffect(_superstarData, _view, 2));
                _effects.Add(new DrawCardsEffect(_superstarData, _view, 1));
                AddDamageEffect();
                return _effects;
                
            case "Lionsault":
                AddGeneralInitialEffects();
                AddDiscardEffect(_opponentData, _opponentData, 1);
                AddDamageEffect();
                return _effects;
            
            case "Tree of Woe":
                // TODO: Probably necessary to add condition
                AddGeneralInitialEffects();
                AddDiscardEffect(_opponentData, _opponentData, 2);
                AddDamageEffect();
                return _effects;
            
            case "Austin Elbow Smash":
                // TODO: Probably necessary to add conditions
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
    {
        _effects.Add(new MakeDamageEffect(_superstarData, _opponentData, _selectedPlay, _view));
    }

    private void AddDiscardEffect(ImportantPlayerData superstarData, ImportantPlayerData deciderData, int numOfCards)
    {
        _effects.Add(new AskToDiscardHandCardsEffect(superstarData, deciderData, _view, numOfCards));
    }
}