using RawDeal.Boundaries;
using RawDeal.Effects;

namespace RawDeal.Cards.Builders;

public interface IEffectBuilder
{
    BoundaryList<Effect> BuildEffects();
}