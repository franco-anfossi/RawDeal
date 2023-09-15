namespace RawDeal.Habilidades_Superstars;

public class HHH : Superstar
{
    public override void HabilidadEspecial()
    {
        throw new InvalidOperationException(SuperstarAbility);
    }
}