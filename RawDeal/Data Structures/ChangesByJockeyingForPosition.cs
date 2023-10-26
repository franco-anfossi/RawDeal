namespace RawDeal.Data_Structures;

public class ChangesByJockeyingForPosition
{
    public int DamageAdded { get; set; }
    public int FortitudeNeeded { get; set; }
    
    public void Reset()
    {
        DamageAdded = 0;
        FortitudeNeeded = 0;
    }
}