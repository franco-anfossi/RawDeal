using RawDeal.Data_Structures;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.WNuevo;

public class ConditionFactory
{
    private readonly List<Conditions> _conditions;
    private readonly IViewablePlayInfo _selectedPlay;
    
    public ConditionFactory(IViewablePlayInfo selectedPlay, IViewablePlayInfo drawnCard)
    {
        _selectedPlay = selectedPlay;
        _conditions = new List<Conditions>();
    }

    public List<Conditions> BuildConditions()
    {
        BuildTypeCondition();
        BuildSubtypeCondition();

        switch (_selectedPlay.CardInfo.Title)
        {
            case "Chop":
                return _conditions;
            case "Arm Bar TakeDown":
                return _conditions;
            case "Collar & Elbow Lockup":
                return _conditions;
            case "Step Aside":
                return _conditions;
            case "Escape Move":
                return _conditions;
            case "Break the Hold":
                return _conditions;
            case "No Chance in Hell":
                return _conditions;
            case "Rolling Takedown":
                _conditions.Add(Conditions.LessThanSevenDamageCondition);
                _conditions.Add(Conditions.UnknownDamageCondition);
                return _conditions;
            case "Knee to the Gut":
                _conditions.Add(Conditions.LessThanSevenDamageCondition);
                _conditions.Add(Conditions.UnknownDamageCondition);
                return _conditions;
            case "Elbow to the Face":
                _conditions.Add(Conditions.LessThanSevenDamageCondition);
                return _conditions;
            case "Manager Interferes":
                return _conditions;
            case "Chyna Interferes":
                return _conditions;
            case "Clean Break":
                _conditions.Add(Conditions.JockeyingForPositionCondition);
                return _conditions;
            case "JockeyingForPosition":
                _conditions.Add(Conditions.JockeyingForPositionCondition);
                return _conditions;
            default:
                return _conditions;
        }
    }
    
    private void BuildTypeCondition()
    {
        switch (_selectedPlay.PlayedAs)
        {
            case "MANEUVER":
                _conditions.Add(Conditions.ManeuverCondition);
                break;
            case "REVERSAL":
                _conditions.Add(Conditions.ReversalCondition);
                break;
            case "ACTION":
                _conditions.Add(Conditions.ActionCondition);
                break;
        }
    }

    private void BuildSubtypeCondition()
    {
        if (_selectedPlay.CardInfo.Subtypes.Contains("Strike"))
        {
            _conditions.Add(Conditions.StrikeCondition);
        }
        else if (_selectedPlay.CardInfo.Subtypes.Contains("Grapple"))
        {
            _conditions.Add(Conditions.GrappleCondition);
        }
        else if (_selectedPlay.CardInfo.Subtypes.Contains("Submission"))
        {
            _conditions.Add(Conditions.SubmissionCondition);
        }
        else if (_selectedPlay.PlayedAs == "REVERSAL")
        {
            if (_selectedPlay.CardInfo.Subtypes[0].Contains("Strike"))
                _conditions.Add(Conditions.StrikeCondition);
            else if (_selectedPlay.CardInfo.Subtypes[0].Contains("Grapple"))
                _conditions.Add(Conditions.GrappleCondition);
            else if (_selectedPlay.CardInfo.Subtypes[0].Contains("Submission"))
                _conditions.Add(Conditions.SubmissionCondition);
            else if (_selectedPlay.CardInfo.Subtypes[0].Contains("Action"))
                _conditions.Add(Conditions.ActionCondition);
        }
    }
}