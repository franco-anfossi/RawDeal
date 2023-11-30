using RawDeal.Boundaries;
using RawDeal.Conditions;
using RawDeal.Data_Structures;
using RawDealView.Formatters;

namespace RawDeal.Cards.Builders;

public class PreconditionBuilder : IConditionBuilder
{
    private readonly IViewablePlayInfo _selectedPlay;
    private readonly BoundaryList<Condition> _conditions;
    private readonly LastCardUsed _lastCardUsed;
    private readonly ImportantPlayerData _playerData;
    
    public PreconditionBuilder(IViewablePlayInfo selectedPlay, 
        LastCardUsed lastCardUsed, ImportantPlayerData playerData)
    {
        _playerData = playerData;
        _selectedPlay = selectedPlay;
        _conditions = new BoundaryList<Condition>();
        _lastCardUsed = lastCardUsed;
    }
    
    public BoundaryList<Condition> BuildConditions()
    {
        switch (_selectedPlay.CardInfo.Title)
        {
            case "Austin Elbow Smash":
                _conditions.Add(new CheckDamageOfLastCard(_selectedPlay, _lastCardUsed, 5));
                return _conditions;
            
            case "Lionsault":
                _conditions.Add(new CheckDamageOfLastCard(_selectedPlay, _lastCardUsed, 4));
                return _conditions;
            
            case "Spit At Opponent":
                _conditions.Add(new MinimumCardsNecessary(_selectedPlay, _playerData, 2));
                return _conditions;
            
            default:
                return _conditions;
        }
    }
}