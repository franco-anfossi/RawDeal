namespace RawDeal;

public interface IJugador
{
    void InicializacionDeAtributos(List<Carta> mazo);
    void SacarCartasAlInicio();
    void SacarCarta();
    void ActualizarDatos();
    object Clonar();
}