using RawDeal.Boundaries;
using RawDeal.Conditions;
using RawDeal.Data_Structures;
using RawDealView.Formatters;

namespace RawDeal.Cards.Builders;

public class ConditionBuilder : IConditionBuilder
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
        string specificReversibleTitle;
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
                specificReversibleTitle = "Jockeying for Position";
                _conditions.Add(new SpecificReversible(_selectedPlay, specificReversibleTitle));
                return _conditions;
            
            case "Spit At Opponent":
                var minimumCardsNecessary = 2;
                _conditions.Add(new MinimumCardsNecessary(_selectedPlay, _playerData, minimumCardsNecessary));
                return _conditions;
            
            case "Belly to Belly Suplex" or "Vertical Suplex" or "Belly to Back Suplex" or 
                "Drop Kick" or "Double Arm DDT" or "Jockeying for Position" or "Irish Whip":
                _conditions.Add(new SpecificReversible(_selectedPlay, _selectedReversal.CardInfo.Title));
                return _conditions;
            
            case "Ensugiri":
                specificReversibleTitle = "Kick";
                _conditions.Add(new SpecificReversible(_selectedPlay, specificReversibleTitle));
                return _conditions;
            
            default:
                return _conditions;
        }
    }
}