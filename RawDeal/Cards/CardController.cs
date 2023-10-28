using RawDeal.Conditions;
using RawDeal.Effects;

namespace RawDeal.Cards;

public class CardController
{
    private readonly List<Condition> _conditions;
    private readonly List<Effect> _effects;
    
    public CardController(List<Effect> effects, List<Condition> conditions)
    {
        _effects = effects;
        _conditions = conditions;
    }
    
    public void PlayCard()
    {
        if (!CheckConditions()) return;
        foreach (var effect in _effects)
        {
            effect.Apply();
        }
    } 
    
    public bool CheckConditions()
    {
        return _conditions.All(condition => condition.Check());
    }
}