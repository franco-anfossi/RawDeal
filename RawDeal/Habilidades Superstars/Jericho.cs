using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Habilidades_Superstars;

public class Jericho : Superstar
{
    private bool _noSePuedeElegirLaHabilidad = false;
    private int _vecesEnLasQueSeAUsadoLaHabilidad = 0;
    public override bool HabilidadEspecial(View view, Superstar oponente)
    {
        View = view;
        if (Hand.Count >= 1 && _vecesEnLasQueSeAUsadoLaHabilidad < 1)
        {
            // Paso 1
            View.SayThatPlayerIsGoingToUseHisAbility(Name, SuperstarAbility);
            List<string> datosDeLasCartas = new List<string>();
            foreach (var carta in Hand)
            {
                string cartaFormateada = Formatter.CardToString(carta);
                datosDeLasCartas.Add(cartaFormateada);
            }
            int indexCartaElegida = View.AskPlayerToSelectACardToDiscard(datosDeLasCartas, Name, Name, 1);
            PasarCartaDeLaManoAlRingside(indexCartaElegida);
            
            // Paso 2
            List<string> datosDeLasCartasOponente = new List<string>();
            foreach (var carta in oponente.Hand)
            {
                string cartaFormateada = Formatter.CardToString(carta);
                datosDeLasCartasOponente.Add(cartaFormateada);
            }

            string nombreOponente = oponente.Name;
            int indexCartaOponenteElegida = View.AskPlayerToSelectACardToDiscard(datosDeLasCartasOponente, nombreOponente, nombreOponente, 1);
            oponente.PasarCartaDeLaManoAlRingside(indexCartaOponenteElegida);
            
            _vecesEnLasQueSeAUsadoLaHabilidad++;
            _noSePuedeElegirLaHabilidad = true;
        }
        return true;
    }
    public override bool NoSePuedeEligirSiUsarLaHabilidad()
    {
        if (Hand.Count >= 1 && _vecesEnLasQueSeAUsadoLaHabilidad < 1) { _noSePuedeElegirLaHabilidad = false; }
        else { _noSePuedeElegirLaHabilidad = true; }
        
        return _noSePuedeElegirLaHabilidad;
    }

    public override void CambiarVisibilidadDeElegirLaHabilidad()
    {
        if (Hand.Count >= 1)
        {
            _vecesEnLasQueSeAUsadoLaHabilidad = 0;
            _noSePuedeElegirLaHabilidad = false;
        }
    }
}