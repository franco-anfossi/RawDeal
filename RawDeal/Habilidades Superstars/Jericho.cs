using Microsoft.VisualBasic.CompilerServices;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Habilidades_Superstars;

public class Jericho : Jugador
{
    private bool _permisoHabilidad = false;
    private int _vecesEnLasQueSeAUsadoLaHabilidad = 0;
    public Jericho(Superstar superstar) : base(superstar)
    {
        Name = superstar.Name;
        Logo = superstar.Logo;
        HandSize = superstar.HandSize;
        SuperstarValue = superstar.SuperstarValue;
        SuperstarAbility = superstar.SuperstarAbility;
    }
    
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
        List<string> datosFormateadosDeLasCartasOponente = Utils.FormatearMazoDeCartas(Oponente.ObtenerMano());
        int indiceCartaOponenteElegida = Oponente.PreguntarPorCartaPorDescartar(datosFormateadosDeLasCartasOponente, 1);
        Oponente.PasarCartaDeLaManoAlRingside(indiceCartaOponenteElegida);
    }
}