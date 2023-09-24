namespace RawDeal.Habilidades_Superstars;

public class Jericho : Superstar
{
    private bool _permisoHabilidad = false;
    private int _vecesEnLasQueSeAUsadoLaHabilidad = 0;
    public override bool EjecutarHabilidadEspecial()
    {
        if (Hand.Count >= 1 && _vecesEnLasQueSeAUsadoLaHabilidad < 1)
        {
            DescartarUnaCartaDeLaManoDeJericho();
            DescartarUnaCartaDeLaManoDelOponente();
            
            _vecesEnLasQueSeAUsadoLaHabilidad++;
            _permisoHabilidad = true;
        }
        return true;
    }
    public override bool ObtenerQueNoSePuedeEligirSiUsarLaHabilidad()
    {
        if (Hand.Count >= 1 && _vecesEnLasQueSeAUsadoLaHabilidad < 1) { _permisoHabilidad = false; }
        else { _permisoHabilidad = true; }
        
        return _permisoHabilidad;
    }

    public override void CambiarVisibilidadDeElegirLaHabilidad()
    {
        if (Hand.Count >= 1)
        {
            _vecesEnLasQueSeAUsadoLaHabilidad = 0;
            _permisoHabilidad = false;
        }
    }

    private void DescartarUnaCartaDeLaManoDeJericho()
    {
        View.SayThatPlayerIsGoingToUseHisAbility(Name, SuperstarAbility);
        List<string> datosFormateadosDeLasCartas = Utils.FormatearMazoDeCartas(Hand);
        int indiceCartaElegida = View.AskPlayerToSelectACardToDiscard(datosFormateadosDeLasCartas, Name, Name, 1);
        PasarCartaDeLaManoAlRingside(indiceCartaElegida);
    }

    private void DescartarUnaCartaDeLaManoDelOponente()
    {
        List<string> datosFormateadosDeLasCartasOponente = Utils.FormatearMazoDeCartas(Oponente.Hand);
        string nombreOponente = Oponente.Name;
        int indiceCartaOponenteElegida = View.AskPlayerToSelectACardToDiscard(datosFormateadosDeLasCartasOponente, nombreOponente, nombreOponente, 1);
        Oponente.PasarCartaDeLaManoAlRingside(indiceCartaOponenteElegida);
    }
}