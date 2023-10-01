using RawDealView.Formatters;

namespace RawDeal;

public static class MazoValidator
{
    private static List<IViewableCardInfo> _grupoCartasIguales;
    private static Mazo _mazoARevisar;
    public static bool ValidadorDeReglasDeMazo(Mazo mazo, ConjuntoCartas conjuntoDeCartas)
    {
        _mazoARevisar = mazo;
        bool condicionUno = ValidarCantidadDeCartasEnMazo() && ValidarRepeticionDeCartas();
        bool condicionDos = ValidarSiHayCartasSoloHeelYSoloFace() && ValidarQueLogoCartasEquivaleLogoSuperstar(conjuntoDeCartas);
        return condicionUno && condicionDos;
    }
    
    private static bool ValidarCantidadDeCartasEnMazo()
    {
        return _mazoARevisar.CartasDelMazo.Count == 60;
    }

    private static bool ValidarRepeticionDeCartas()
    {
        var agrupacionesDeCartas = _mazoARevisar.CartasDelMazo.GroupBy(carta => carta.Title);
        foreach (var grupoDeCartasConMismoNombre in agrupacionesDeCartas)
        {
            _grupoCartasIguales = grupoDeCartasConMismoNombre.ToList();
            int maxRepeticionesPermitidas = ObtenerMaximasRepeticionesPermitidas();
            if (_grupoCartasIguales.Count > maxRepeticionesPermitidas) {return false;}
        }
        return true;
    }
    
    private static bool ValidarSiHayCartasSoloHeelYSoloFace()
    {
        bool existeHeel = _mazoARevisar.CartasDelMazo.Any(c => c.Subtypes.Contains("Heel"));
        bool existeFace = _mazoARevisar.CartasDelMazo.Any(c => c.Subtypes.Contains("Face"));
        return !(existeHeel && existeFace);
    }

    private static bool ValidarQueLogoCartasEquivaleLogoSuperstar(ConjuntoCartas conjuntoDeCartas)
    {
        var logoSuperstarEnRevision = _mazoARevisar.JugadorDelMazo.ObtenerLogo();
        foreach (var superstar in conjuntoDeCartas.SuperstarsPosibles)
        {
            string logoOtroSuperstar = superstar.ObtenerLogo();
            if (!RevisarPorLogosLasCartasDelMazo(logoOtroSuperstar, logoSuperstarEnRevision)) { return false;}
        }
        
        return true;
    }

    private static bool RevisarPorLogosLasCartasDelMazo(string logoOtroSuperstar, string logoSuperstarEnRevision)
    {
        if (logoOtroSuperstar != logoSuperstarEnRevision)
        {
            if (_mazoARevisar.CartasDelMazo.Any(c => c.Subtypes.Contains(logoOtroSuperstar))) { return false; }
        }
        return true;
    }
    private static int ObtenerMaximasRepeticionesPermitidas()
    {
        int maxRepeticionesPermitidas = 3;
        maxRepeticionesPermitidas = ValidarRepeticionUnique(maxRepeticionesPermitidas);
        maxRepeticionesPermitidas = ValidarRepeticionSetUp(maxRepeticionesPermitidas);

        return maxRepeticionesPermitidas;
    }
    
    private static int ValidarRepeticionUnique(int maxRepeticionesPermitido)
    {
        if (_grupoCartasIguales.Any(c => c.Subtypes.Contains("Unique"))) 
            maxRepeticionesPermitido = 1;
        return maxRepeticionesPermitido;
    }

    private static int ValidarRepeticionSetUp(int maxRepeticionesPermitido)
    {
        if (_grupoCartasIguales.Any(c => c.Subtypes.Contains("SetUp"))) 
            maxRepeticionesPermitido = 70;
        return maxRepeticionesPermitido;
    }
}

