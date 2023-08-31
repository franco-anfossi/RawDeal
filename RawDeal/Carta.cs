namespace RawDeal;

public class Carta
{
    public string Title { get; set; }
    public List<string> Types { get; set; }
    public List<string> Subtypes { get; set; }
    public string Fortitude { get; set; }
    public string Damage { get; set; }
    public string StunValue { get; set; }
    public string CardEffect { get; set; }

    public override string ToString()
    {
        string text = $"{Title} - ";
        foreach (var lol in Subtypes)
        {
            text += lol + ", ";
        }
        return text;
    }
}