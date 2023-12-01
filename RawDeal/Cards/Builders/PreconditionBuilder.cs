using RawDeal.Boundaries;
using RawDeal.Conditions;
using RawDeal.Data_Structures;
using RawDealView.Formatters;

namespace RawDeal.Cards.Builders;

public class PreconditionBuilder : IConditionBuilder
{
    private readonly IViewablePlayInfo _selectedPlay;
    private readonly BoundaryList<Condition> _conditions;
    private readonly ImportantPlayerData _playerData;
    
    public PreconditionBuilder(IViewablePlayInfo selectedPlay, ImportantPlayerData playerData)
    {
        _playerData = playerData;
        _selectedPlay = selectedPlay;
        _conditions = new BoundaryList<Condition>();
    }
    
    public BoundaryList<Condition> BuildConditions()
    {
        int damageToCompare;
        string lastCardType;
        switch (_selectedPlay.CardInfo.Title)
        {
            case "Austin Elbow Smash":
                damageToCompare = 5;
                lastCardType = "MANEUVER";
                MinimumFortitudeCondition();
                _conditions.Add(new DamageOfLastCard(_playerData, _selectedPlay, damageToCompare, lastCardType));
                return _conditions;
            
            case "Lionsault":
                damageToCompare = 4;
                lastCardType = "MANEUVER";
                MinimumFortitudeCondition();
                _conditions.Add(new DamageOfLastCard(_playerData, _selectedPlay, damageToCompare, lastCardType));
                return _conditions;
            
            case "Spit At Opponent":
                int minimumCardsNecessary = 2;
                MinimumFortitudeCondition();
                _conditions.Add(new MinimumCardsNecessary(_selectedPlay, _playerData, minimumCardsNecessary));
                return _conditions;
            
            case "Undertaker's Tombstone Piledriver":
                SelectTombstoneCondition();
                return _conditions;
            
            default:
                MinimumFortitudeCondition();
                return _conditions;
        }
    }
    
    private void SelectTombstoneCondition()
    {
        int cardFortitude = Convert.ToInt32(_selectedPlay.CardInfo.Fortitude);
        
        if (_selectedPlay.PlayedAs == "MANEUVER")
            _conditions.Add(new MinimumFortitude(_selectedPlay, _playerData, cardFortitude));
        
        else if (_selectedPlay.PlayedAs == "ACTION")
            _conditions.Add(new MinimumFortitude(_selectedPlay, _playerData, cardFortitude - 30));
    }
    
    private void MinimumFortitudeCondition()
    {
        int cardFortitude = Convert.ToInt32(_selectedPlay.CardInfo.Fortitude);
        _conditions.Add(new MinimumFortitude(_selectedPlay, _playerData, cardFortitude));
    }
}