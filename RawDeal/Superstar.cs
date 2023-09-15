namespace RawDeal;

public abstract class Superstar
{
    public string Name { get; private set; }
    public string Logo { get; private set; }
    public int HandSize { get; private set; }
    public int SuperstarValue { get; private set; }
    public string SuperstarAbility { get; private set; }
    
    public override string ToString()
    {
        return $"{Name}";
    }
    public virtual void HabilidadEspecial()
    {
        Console.WriteLine("Habilidad Especial");
    }
}