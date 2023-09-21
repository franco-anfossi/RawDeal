using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Habilidades_Superstars;

public class TheRock : Superstar
{
    public override bool HabilidadEspecial(View view, Superstar oponente)
    {
        View = view;
        if (Ringside.Count != 0)
        {
            bool respuesta = View.DoesPlayerWantToUseHisAbility(Name);
            if (respuesta)
            {
                View.SayThatPlayerIsGoingToUseHisAbility(Name, SuperstarAbility);

                List<string> datosDeLasCartas = new List<string>();
                foreach (var carta in Ringside)
                {
                    string cartaFormateada = Formatter.CardToString(carta);
                    datosDeLasCartas.Add(cartaFormateada);
                }

                int indexCarta = View.AskPlayerToSelectCardsToRecover(Name, 1, datosDeLasCartas);
                PasarCartaDeRingsideAlArsenal(indexCarta);
            }
        }
        return true;
    }
}