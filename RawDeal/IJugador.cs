namespace RawDeal;

public interface IJugador
{
    void IngresarMazo(List<Carta> mazo);
    void SacarCartasAlInicio();
    void SacarCarta();
    void ActualizarDatos();
}