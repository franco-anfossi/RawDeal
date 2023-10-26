using RawDeal.Effects;

namespace RawDeal.Cards;

public class CardController
{
    private readonly List<Effect> _effectsToApply;
    
    public CardController(List<Effect> effectsToApply)
    {
        _effectsToApply = effectsToApply;
    }
    
    public void ApplyEffects()
    {
        foreach (var effect in _effectsToApply)
        {
            effect.Apply();
        }
    }
}