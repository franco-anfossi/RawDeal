using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Habilidades_Superstars;

public class Jericho : Superstar
{
    public new bool NoPuedeElegirUsarSuHabilidad = false;
    private int _vecesEnLasQueSeAUsadoLaHabilidad = 0;
    public override bool HabilidadEspecial(View view, Superstar oponente)
    {
        _view = view;
        if (Hand.Count >= 1 && _vecesEnLasQueSeAUsadoLaHabilidad < 1)
        {
            // Paso 1
            _view.SayThatPlayerIsGoingToUseHisAbility(Name, SuperstarAbility);
            List<string> datosDeLasCartas = new List<string>();
            foreach (var carta in Hand)
            {
                string cartaFormateada = Formatter.CardToString(carta);
                datosDeLasCartas.Add(cartaFormateada);
            }
            int indexCartaElegida = _view.AskPlayerToSelectACardToDiscard(datosDeLasCartas, Name, Name, 1);
            PasarCartaDeLaManoAlRingside(indexCartaElegida);
            
            // Paso 2
            List<string> datosDeLasCartasOponente = new List<string>();
            foreach (var carta in oponente.Hand)
            {
                string cartaFormateada = Formatter.CardToString(carta);
                datosDeLasCartasOponente.Add(cartaFormateada);
            }

            string nombreOponente = oponente.Name;
            int indexCartaOponenteElegida = _view.AskPlayerToSelectACardToDiscard(datosDeLasCartasOponente, nombreOponente, nombreOponente, 1);
            oponente.PasarCartaDeLaManoAlRingside(indexCartaOponenteElegida);
            _vecesEnLasQueSeAUsadoLaHabilidad++;
        }
        NoPuedeElegirUsarSuHabilidad = true;
        return true;
    }
}