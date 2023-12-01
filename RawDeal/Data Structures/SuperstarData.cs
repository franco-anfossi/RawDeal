namespace RawDeal.Data_Structures;

public class SuperstarData
{
    public string Name { get; set; }
    public string Logo { get; set; }
    public int HandSize { get; set; }
    public int SuperstarValue { get; set; }
    public string SuperstarAbility { get; set; }
    public int Fortitude { get; set; }
    
    public object Clone()
        => MemberwiseClone();
}