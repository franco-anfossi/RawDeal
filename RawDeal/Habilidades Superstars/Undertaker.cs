namespace RawDeal.Habilidades_Superstars;

public class Undertaker : Superstar
{
    private bool _permisoHabilidad;
    private int _vecesEnLasQueSeAUsadoLaHabilidad;
    public override bool EjecutarHabilidadEspecial()
    {
        if (_vecesEnLasQueSeAUsadoLaHabilidad < 1 && Hand.Count >= 2)
        {
            View.SayThatPlayerIsGoingToUseHisAbility(Name, SuperstarAbility);
            
            DescartarDosCartasDeLaMano();
            RobarUnaCartaDelRingside();
            
            _vecesEnLasQueSeAUsadoLaHabilidad++;
            _permisoHabilidad = true;
        }
        return true;
    }
    public override bool ObtenerQueNoSePuedeEligirSiUsarLaHabilidad()
    {
        if (Hand.Count >= 2 && _vecesEnLasQueSeAUsadoLaHabilidad < 1) { _permisoHabilidad = false; }
        else { _permisoHabilidad = true; }
        
        return _permisoHabilidad;
    }
    public override void CambiarVisibilidadDeElegirLaHabilidad()
    {
        if (Hand.Count >= 2)
        {
            _vecesEnLasQueSeAUsadoLaHabilidad = 0;
            _permisoHabilidad = false;
        }
    }

    private void DescartarDosCartasDeLaMano()
    {
        for (int cartasPorSacar = 2; cartasPorSacar > 0; cartasPorSacar--)
        {
            List<string> datosFormateadosDeLasCartasDeLaMano = Utils.FormatearMazoDeCartas(Hand);
            int indiceCartaElegida = View.AskPlayerToSelectACardToDiscard(datosFormateadosDeLasCartasDeLaMano, Name, Name, cartasPorSacar);
            PasarCartaDeLaManoAlRingside(indiceCartaElegida);
        }
    }

    private void RobarUnaCartaDelRingside()
    {
        List<string> datosFormateadosDeLasCartasDelRingside = Utils.FormatearMazoDeCartas(Ringside);
        int indiceCartaRingsideElegida = View.AskPlayerToSelectCardsToPutInHisHand(Name, 1, datosFormateadosDeLasCartasDelRingside);
        PasarCartaDelRingsideALaMano(indiceCartaRingsideElegida);
    }
}