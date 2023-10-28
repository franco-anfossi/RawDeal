using RawDeal.Effects;

namespace RawDeal.Cards.Builders;

public interface IEffectBuilder
{
    List<Effect> BuildEffects();
}