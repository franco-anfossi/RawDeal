namespace RawDeal.Conditions;

public abstract class Condition
{
    protected Condition() { }

    public abstract bool Check();
}