using RawDeal.Boundaries;
using RawDeal.Conditions;

namespace RawDeal.Cards.Builders;

public interface IConditionBuilder
{
    public BoundaryList<Condition> BuildConditions();
}