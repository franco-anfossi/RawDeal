using RawDealView.Formatters;

namespace RawDeal;

public interface IJugador
{
    bool EjecutarHabilidadEspecial();
    void InicializarLosAtributosNecesarios(List<IViewableCardInfo> mazo);
    void SacarCartasAlInicio();
    void SacarCarta();
    void ActualizarDatos();
    object Clonar();
}