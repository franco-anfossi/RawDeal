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
    private readonly List<Effect> _effects;
    private readonly View _view;
    
    public GeneralEffectBuilder (ImportantPlayerData superstarData, ImportantPlayerData opponentData,
        IViewablePlayInfo selectedPlay, View view)
    {
        _view = view;
        _superstarData = superstarData;
        _opponentData = opponentData;
        _selectedPlay = selectedPlay;
        _effects = new List<Effect>();
    }
    
    public List<Effect> BuildEffects()
    {
        switch (_selectedPlay.CardInfo.Title)
        {
            case "Chop" or "Arm Bar Takedown" or "Collar & Elbow Lockup":
                _effects.Add(new ReverseFromHandEffect(_superstarData, _opponentData, _selectedPlay, _view));
                _effects.Add(new BasicHybridEffect(_superstarData, _opponentData, _selectedPlay, _view));
                return _effects;
            
            case "Jockeying for Position":
                _effects.Add(new ReverseFromHandEffect(_superstarData, _opponentData, _selectedPlay, _view));
                _effects.Add(new SuccessfullyPlayedCardEffect(_superstarData, _selectedPlay, _view));
                _effects.Add(new JockeyingForPositionEffect(_superstarData, _opponentData, _view));
                return _effects;
            
            case "Head Butt" or "Arm Drag" or "Arm Bar":
                _effects.Add(new ReverseFromHandEffect(_superstarData, _opponentData, _selectedPlay, _view));
                _effects.Add(new SuccessfullyPlayedCardEffect(_superstarData, _selectedPlay, _view));
                _effects.Add(new AskToDiscardHandCardsEffect(
                    _superstarData, _superstarData, _view, 1));
                _effects.Add(new MakeDamageEffect(_superstarData, _opponentData, _selectedPlay, _view));
                return _effects;
            
            case "Pump Handle Slam":
                _effects.Add(new ReverseFromHandEffect(_superstarData, _opponentData, _selectedPlay, _view));
                _effects.Add(new SuccessfullyPlayedCardEffect(_superstarData, _selectedPlay, _view));
                _effects.Add(new AskToDiscardHandCardsEffect(
                    _opponentData, _opponentData, _view, 2));
                _effects.Add(new MakeDamageEffect(_superstarData, _opponentData, _selectedPlay, _view));
                return _effects;
            
            case "Bear Hug" or "Choke Hold" or "Ankle Lock" or "Spinning Heel Kick" or "Figure Four Leg Lock" or 
                "Samoan Drop" or "Boston Crab" or "Power Slam" or "Torture Rack":
                _effects.Add(new ReverseFromHandEffect(_superstarData, _opponentData, _selectedPlay, _view));
                _effects.Add(new SuccessfullyPlayedCardEffect(_superstarData, _selectedPlay, _view));
                _effects.Add(new AskToDiscardHandCardsEffect(
                    _opponentData, _opponentData, _view, 1));
                _effects.Add(new MakeDamageEffect(_superstarData, _opponentData, _selectedPlay, _view));
                return _effects;
            
            case "Bulldog":
                _effects.Add(new ReverseFromHandEffect(_superstarData, _opponentData, _selectedPlay, _view));
                _effects.Add(new SuccessfullyPlayedCardEffect(_superstarData, _selectedPlay, _view));
                _effects.Add(new AskToDiscardHandCardsEffect(
                    _superstarData, _superstarData, _view, 1));
                _effects.Add(new AskToDiscardHandCardsEffect(
                    _opponentData, _superstarData, _view, 1));
                _effects.Add(new MakeDamageEffect(_superstarData, _opponentData, _selectedPlay, _view));
                return _effects;
            
            case "Headlock Takedown" or "Standing Side Headlock":
                _effects.Add(new ReverseFromHandEffect(_superstarData, _opponentData, _selectedPlay, _view));
                _effects.Add(new SuccessfullyPlayedCardEffect(_superstarData, _selectedPlay, _view));
                _effects.Add(new DrawCardsEffect(_opponentData, _view, 1));
                return _effects;
            
            default:
                _effects.Add(new ReverseFromHandEffect(_superstarData, _opponentData, _selectedPlay, _view));
                _effects.Add(new SuccessfullyPlayedCardEffect(_superstarData, _selectedPlay, _view));
                _effects.Add(new MakeDamageEffect(_superstarData, _opponentData, _selectedPlay, _view));
                return _effects;
        }
    }
}