namespace RawDeal.Habilidades_Superstars;

public class TheRock : Superstar
{
    private bool _respuestaDeLaHabilidad;
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