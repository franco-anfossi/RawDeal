namespace RawDeal.Data_Structures;

public class ChangesByIrishWhip
{
    public int DamageAdded { get; set; }
    
    public void Reset()
        => DamageAdded = 0;
}