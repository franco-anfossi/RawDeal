using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Habilidades_Superstars;

public class TheRock : Jugador
{
    private bool _respuestaDeLaHabilidad;
    public TheRock(Superstar superstar) : base(superstar)
    {
        Name = superstar.Name;
        Logo = superstar.Logo;
        HandSize = superstar.HandSize;
        SuperstarValue = superstar.SuperstarValue;
        SuperstarAbility = superstar.SuperstarAbility;
    }
    
    public override bool EjecutarHabilidadEspecial()
    {
        if (Ringside.Count != 0)
        {
            _respuestaDeLaHabilidad = View.DoesPlayerWantToUseHisAbility(Name);
            JugarHabilidadDeTheRock();
        }
        return true;
    }

    private void JugarHabilidadDeTheRock()
    {
        if (_respuestaDeLaHabilidad)
        {
            View.SayThatPlayerIsGoingToUseHisAbility(Name, SuperstarAbility);
            List<string> datosFormateadosDeLasCartas = Utils.FormatearMazoDeCartas(Ringside);
            int indiceCartaSeleccionada = View.AskPlayerToSelectCardsToRecover(Name, 1, datosFormateadosDeLasCartas);
            PasarCartaDesdeUnMazoHastaFondoArsenal(Ringside, indiceCartaSeleccionada);
        }
    }
}