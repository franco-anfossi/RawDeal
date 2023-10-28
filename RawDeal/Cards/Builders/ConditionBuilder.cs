using RawDeal.Boundaries;
using RawDeal.Conditions;
using RawDeal.Data_Structures;
using RawDealView.Formatters;

namespace RawDeal.Cards.Builders;

public class ConditionBuilder
{
    private readonly IViewablePlayInfo _selectedPlay;
    private readonly IViewablePlayInfo _selectedReversal;
    private readonly ImportantPlayerData _playerData;
    private readonly BoundaryList<Condition> _conditions;
    
    public ConditionBuilder(ImportantPlayerData playerData, 
        IViewablePlayInfo selectedPlay, IViewablePlayInfo selectedReversal)
    {
        _playerData = playerData;
        _selectedPlay = selectedPlay;
        _selectedReversal = selectedReversal;
        _conditions = new BoundaryList<Condition>();
    }
    
    public BoundaryList<Condition> BuildConditions()
    {
        switch (_selectedReversal.CardInfo.Title)
        {
            case "Step Aside" or "Escape Move" or "Break the Hold" or "No Chance in Hell":
                _conditions.Add(new CorrectSubtype(_selectedPlay, _selectedReversal));
                return _conditions;
            
            case "Rolling Takedown" or "Knee to the Gut":
                _conditions.Add(new CorrectSubtype(_selectedPlay, _selectedReversal));
                _conditions.Add(new LessThanSevenDamage(_playerData, _selectedPlay));
                return _conditions;
            
            case "Elbow to the Face":
                _conditions.Add(new LessThanSevenDamage(_playerData, _selectedPlay));
                return _conditions;
            
            case "Clean Break":
                _conditions.Add(new IsJockeyingForPosition(_selectedPlay));
                return _conditions;
            
            case "Jockeying for Position":
                _conditions.Add(new IsJockeyingForPosition(_selectedPlay));
                return _conditions;
            
            default:
                return _conditions;
        }
    }
}