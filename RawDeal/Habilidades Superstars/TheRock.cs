using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Habilidades_Superstars;

public class TheRock : Superstar
{
    public new bool NoPuedeElegirUsarSuHabilidad = true;
    public override bool HabilidadEspecial(View view, Superstar oponente)
    {
        _view = view;
        if (Ringside.Count != 0)
        {
            bool respuesta = _view.DoesPlayerWantToUseHisAbility(Name);
            if (respuesta)
            {
                _view.SayThatPlayerIsGoingToUseHisAbility(Name, SuperstarAbility);

                List<string> datosDeLasCartas = new List<string>();
                foreach (var carta in Ringside)
                {
                    string cartaFormateada = Formatter.CardToString(carta);
                    datosDeLasCartas.Add(cartaFormateada);
                }

                int indexCarta = _view.AskPlayerToSelectCardsToRecover(Name, 1, datosDeLasCartas);
                PasarCartaDeRingsideAlArsenal(indexCarta);
                return false;
            }

            return true;
        }

        return true;
    }
}