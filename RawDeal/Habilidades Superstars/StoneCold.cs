namespace RawDeal.Habilidades_Superstars;

public class StoneCold : Superstar
{
    private bool _permisoHabilidad = false;
    private int _vecesEnLasQueSeAUsadoLaHabilidad = 0;
    public override bool EjecutarHabilidadEspecial()
    {
        if (_vecesEnLasQueSeAUsadoLaHabilidad < 1 && Arsenal.Count >= 0)
        {
            EjecutarPasosDeLaHabilidad();
            _vecesEnLasQueSeAUsadoLaHabilidad++;
            _permisoHabilidad = true;
        }
        return true;
    }
    public override bool ObtenerQueNoSePuedeEligirSiUsarLaHabilidad()
    {
        return _permisoHabilidad;
    }
    public override void CambiarVisibilidadDeElegirLaHabilidad()
    {
        _vecesEnLasQueSeAUsadoLaHabilidad = 0;
        _permisoHabilidad = false;
    }

    private void EjecutarPasosDeLaHabilidad()
    {
        View.SayThatPlayerIsGoingToUseHisAbility(Name, SuperstarAbility);
        View.SayThatPlayerDrawCards(Name, 1);
        SacarCarta();
        List<string> datosFormateadosDeLasCartas = Utils.FormatearMazoDeCartas(Hand);
        int indiceDeLaCartaElegida = View.AskPlayerToReturnOneCardFromHisHandToHisArsenal(Name, datosFormateadosDeLasCartas);
        PasarCartaDesdeUnMazoHastaFondoArsenal(Hand, indiceDeLaCartaElegida);
    }
}