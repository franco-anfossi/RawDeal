using RawDeal.Boundaries;
using RawDeal.Data_Structures;
using RawDeal.Effects;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Cards.Builders;

public class ActionEffectBuilder : IEffectBuilder
{
    private readonly ImportantPlayerData _superstarData;
    private readonly ImportantPlayerData _opponentData;
    private readonly IViewablePlayInfo _selectedPlay;
    private readonly BoundaryList<Effect> _effects;
    private readonly View _view;
    
    public ActionEffectBuilder (ImportantPlayerData superstarData, ImportantPlayerData opponentData,
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
        int numCardsToDiscard;
        int numCardsToDraw;
        int numCardsToRecover;
        
        switch (_selectedPlay.CardInfo.Title)
        {
            case "Chop" or "Arm Bar Takedown" or "Collar & Elbow Lockup" or "Undertaker's Tombstone Piledriver":
                _effects.Add(new ReverseFromHandEffect(_superstarData, _opponentData, _selectedPlay, _view));
                _effects.Add(new MustDiscardHandCardEffect(_superstarData, _view, _selectedPlay.CardInfo));
                _effects.Add(new ResetIrishBonus(_superstarData, _opponentData, _view));
                _effects.Add(new UpdateLastCard(_superstarData, _opponentData, _selectedPlay, _view));
                return _effects;
            
            case "Jockeying for Position":
                AddGeneralInitialEffects();
                _effects.Add(new JockeyingForPositionEffect(_superstarData, _opponentData, _view));
                return _effects;
            
            case "Offer Handshake":
                numCardsToDraw = 3;
                numCardsToDiscard = 1;
                AddGeneralInitialEffects();
                _effects.Add(new AskToDrawEffect(_superstarData, _view, numCardsToDraw));
                AddDiscardEffect(_superstarData, _superstarData, numCardsToDiscard);
                AddDamageEffect();
                return _effects;
            
            case "Spit At Opponent":
                numCardsToDiscard = 1;
                AddGeneralInitialEffects();
                AddDiscardEffect(_superstarData, _superstarData, numCardsToDiscard);
                numCardsToDiscard = 4;
                AddDiscardEffect(_opponentData, _opponentData, numCardsToDiscard);
                AddDamageEffect();
                return _effects;
            
            case "Puppies! Puppies!":
                numCardsToDraw = 2;
                numCardsToRecover = 5;
                AddGeneralInitialEffects();
                _effects.Add(new RecoverEffect(_superstarData, _view, numCardsToRecover));
                _effects.Add(new DrawCardsEffect(_superstarData, _view, numCardsToDraw));
                AddDamageEffect();
                return _effects;
            
            case  "Recovery":
                numCardsToRecover = 2;
                numCardsToDraw = 1;
                AddGeneralInitialEffects();
                _effects.Add(new RecoverEffect(_superstarData, _view, numCardsToRecover));
                _effects.Add(new DrawCardsEffect(_superstarData, _view, numCardsToDraw));
                AddDamageEffect();
                return _effects;
            
            case "Irish Whip":
                AddGeneralInitialEffects();
                _effects.Add(new IrishWhipEffect(_superstarData, _view));
                _effects.Add(new UpdateLastCard(_superstarData, _opponentData, _selectedPlay, _view));
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