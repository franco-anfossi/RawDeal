using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Habilidades_Superstars;

public class Undertaker : Superstar
{
    private bool _noSePuedeElegirLaHabilidad = false;
    private int _vecesEnLasQueSeAUsadoLaHabilidad = 0;
    public override bool HabilidadEspecial(View view, Superstar oponente)
    {
        View = view;
        if (_vecesEnLasQueSeAUsadoLaHabilidad < 1 && Hand.Count >= 2)
        {
            View.SayThatPlayerIsGoingToUseHisAbility(Name, SuperstarAbility);
            
            for (int cartasPorSacar = 2; cartasPorSacar > 0; cartasPorSacar--)
            {
                List<string> datosDeLasCartasDeLaMano = new List<string>();
                foreach (var carta in Hand)
                {
                    string cartaFormateada = Formatter.CardToString(carta);
                    datosDeLasCartasDeLaMano.Add(cartaFormateada);
                }
                
                int indexCartaElegida = View.AskPlayerToSelectACardToDiscard(datosDeLasCartasDeLaMano, Name, Name, cartasPorSacar);
                PasarCartaDeLaManoAlRingside(indexCartaElegida);
            }

            List<string> datosDeLasCartasDelRingside = new List<string>();
            foreach (var carta in Ringside)
            {
                string cartaFormateada = Formatter.CardToString(carta);
                datosDeLasCartasDelRingside.Add(cartaFormateada);
            }

            int indexCartaRingsideElegida = View.AskPlayerToSelectCardsToPutInHisHand(Name, 1, datosDeLasCartasDelRingside);
            PasarCartaDelRingsideALaMano(indexCartaRingsideElegida);
            
            _vecesEnLasQueSeAUsadoLaHabilidad++;
            _noSePuedeElegirLaHabilidad = true;
        }
        return true;
    }
    public override bool NoSePuedeEligirSiUsarLaHabilidad()
    {
        if (Hand.Count >= 2 && _vecesEnLasQueSeAUsadoLaHabilidad < 1) { _noSePuedeElegirLaHabilidad = false; }
        else { _noSePuedeElegirLaHabilidad = true; }
        
        return _noSePuedeElegirLaHabilidad;
    }
    public override void CambiarVisibilidadDeElegirLaHabilidad()
    {
        if (Hand.Count >= 2)
        {
            _vecesEnLasQueSeAUsadoLaHabilidad = 0;
            _noSePuedeElegirLaHabilidad = false;
        }
    }
}