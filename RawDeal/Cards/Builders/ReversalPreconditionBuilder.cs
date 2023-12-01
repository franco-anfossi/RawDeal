using RawDeal.Boundaries;
using RawDeal.Conditions;
using RawDeal.Data_Structures;
using RawDealView.Formatters;

namespace RawDeal.Cards.Builders;

public class ReversalPreconditionBuilder : IConditionBuilder
{
    private readonly IViewablePlayInfo _selectedPlay;
    private readonly BoundaryList<Condition> _conditions;
    private readonly ImportantPlayerData _playerData;
    
    public ReversalPreconditionBuilder(IViewablePlayInfo selectedPlay, ImportantPlayerData playerData)
    {
        _playerData = playerData;
        _selectedPlay = selectedPlay;
        _conditions = new BoundaryList<Condition>();
    }   
    
    public BoundaryList<Condition> BuildConditions()
    {
        switch (_selectedPlay.CardInfo.Title)
        {
            case "Tree of Woe":
                _conditions.Add(new NotReversible(_selectedPlay));
                return _conditions;
            
            case "Austin Elbow Smash":
                _conditions.Add(new NotReversible(_selectedPlay));
                return _conditions;
            
            default:
                return _conditions;
        }
    }
}