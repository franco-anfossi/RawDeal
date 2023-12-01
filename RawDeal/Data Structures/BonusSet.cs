namespace RawDeal.Data_Structures;

public class BonusSet
{
    public ChangesByJockeyingForPosition ChangesByJockeyingForPosition { get; } = new();
    public MankindDamageChange MankindBonusDamageChange { get; } = new();
    public ChangesByIrishWhip ChangesByIrishWhip { get; } = new();
}