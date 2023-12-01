using RawDeal.Boundaries;
using RawDeal.Conditions;
using RawDeal.Effects;

namespace RawDeal.Cards;

public class CardController
{
    private readonly BoundaryList<Condition> _conditions;
    private readonly BoundaryList<Effect> _effects;
    
    public CardController(BoundaryList<Effect> effects, BoundaryList<Condition> conditions)
    {
        _effects = effects;
        _conditions = conditions;
    }
    
    public void PlayCard()
    {
        if (!CheckConditions()) return;
        foreach (var effect in _effects)
            effect.Apply();
    } 
    
    public bool CheckConditions()
        => _conditions.All(condition => condition.Check());
}