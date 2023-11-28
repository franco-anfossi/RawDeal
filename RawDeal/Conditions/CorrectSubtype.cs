using RawDealView.Formatters;

namespace RawDeal.Conditions;

public class CorrectSubtype : Condition
{
    private readonly IViewablePlayInfo _selectedCard;
    private readonly string _selectedReversalCardSubtype;
    
    public CorrectSubtype(IViewablePlayInfo selectedPlay, IViewablePlayInfo selectedReversalPlay)
    {
        _selectedCard = selectedPlay;
        _selectedReversalCardSubtype = selectedReversalPlay.CardInfo.Subtypes[0];
    }
    
    public override bool Check()
    {
        var isStrikeOrGrapple = CheckMatchingSubtype("Strike") || CheckMatchingSubtype("Grapple");
        var isSubmissionOrAction = CheckMatchingSubtype("Submission") || CheckMatchingTypeAndSubtype("Action");
        return isStrikeOrGrapple || isSubmissionOrAction;
    }

    private bool CheckMatchingSubtype(string subtype)
    {
        return _selectedCard.CardInfo.Subtypes.Contains(subtype) && _selectedReversalCardSubtype.Contains(subtype);
    }

    private bool CheckMatchingTypeAndSubtype(string type)
    {
        return _selectedCard.PlayedAs == type.ToUpper() && _selectedReversalCardSubtype.Contains(type);
    }

}