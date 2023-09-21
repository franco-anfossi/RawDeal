using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Habilidades_Superstars;

public class StoneCold : Superstar
{
    private bool _noSePuedeElegirLaHabilidad = false;
    private int _vecesEnLasQueSeAUsadoLaHabilidad = 0;
    public override bool HabilidadEspecial(View view, Superstar oponente)
    {
        View = view;
        if (_vecesEnLasQueSeAUsadoLaHabilidad < 1 && Arsenal.Count >= 0)
        {
            View.SayThatPlayerIsGoingToUseHisAbility(Name, SuperstarAbility);
            View.SayThatPlayerDrawCards(Name, 1);
            SacarCarta();
            
            List<string> datosDeLasCartas = new List<string>();
            foreach (var carta in Hand)
            {
                string cartaFormateada = Formatter.CardToString(carta);
                datosDeLasCartas.Add(cartaFormateada);
            }
            
            int idDeLaCartaElegida = View.AskPlayerToReturnOneCardFromHisHandToHisArsenal(Name, datosDeLasCartas);
            PasarCartaDeLaManoAlArsenal(idDeLaCartaElegida);
            _vecesEnLasQueSeAUsadoLaHabilidad++;
            _noSePuedeElegirLaHabilidad = true;
        }
        return true;
    }
    public override bool NoSePuedeEligirSiUsarLaHabilidad()
    {
        return _noSePuedeElegirLaHabilidad;
    }
    public override void CambiarVisibilidadDeElegirLaHabilidad()
    {
        _vecesEnLasQueSeAUsadoLaHabilidad = 0;
        _noSePuedeElegirLaHabilidad = false;
    }
}