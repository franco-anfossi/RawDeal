namespace RawDeal;

using System.Linq;

public static class MazoValidator
{
    public static bool ValidadorDeReglasDeMazo(Mazo mazo, ConjuntoCartas todasCartas)
    {
        bool condicionUno = ValidarCantidadDeCartasEnMazo(mazo) && ValidarRepeticionDeCartas(mazo);
        bool condicionDos = ValidarSoloHeelSoloFace(mazo) && ValidarLogoCartasEquivaleLogoSuperstar(mazo, todasCartas);
        bool condicionTres = ValidarSoloUnSuperstar(mazo);
        return condicionUno && condicionDos;
    }
    
    private static bool ValidarCantidadDeCartasEnMazo(Mazo mazo)
    {
        return mazo.SuperstarsInicial.Count == 1 && mazo.Cartas.Count == 60;
    }

    private static bool ValidarRepeticionDeCartas(Mazo mazo)
    {
        var gruposCartas = mazo.Cartas.GroupBy(carta => carta.Title);
        foreach (var grupo in gruposCartas)
        {
            int maxPermitido = 3;
            var cartasIguales = grupo.ToList();
            maxPermitido = ValidarRepeticionUnique(cartasIguales, maxPermitido);
            maxPermitido = ValidarRepeticionSetUp(cartasIguales, maxPermitido);

            if (cartasIguales.Count > maxPermitido)
                return false;
        }
        return true;
    }
    
    private static bool ValidarSoloHeelSoloFace(Mazo mazo)
    {
        bool existeHeel = mazo.Cartas.Any(c => c.Subtypes.Contains("Heel"));
        bool existeFace = mazo.Cartas.Any(c => c.Subtypes.Contains("Face"));
        return !(existeHeel && existeFace);
    }

    private static bool ValidarLogoCartasEquivaleLogoSuperstar(Mazo mazo, ConjuntoCartas todasCartas)
    {
        var superstarLogo = mazo.SuperstarsInicial[0].Logo;
        foreach (var superstar in todasCartas.Superstars)
        {
            if (superstar.Logo != superstarLogo)
                if (mazo.Cartas.Any(c => c.Subtypes.Contains(superstar.Logo))) { return false; }
        }
        
        return true;
    }

    private static bool ValidarSoloUnSuperstar(Mazo mazo)
    {
        if (mazo.SuperstarsInicial.Count != 1)
            return false;
        return true;
    } 
    private static int ValidarRepeticionUnique(List<Carta> cartasIguales, int maxPermitido)
    {
        if (cartasIguales.Any(c => c.Subtypes.Contains("Unique"))) 
            maxPermitido = 1;
        return maxPermitido;
    }

    private static int ValidarRepeticionSetUp(List<Carta> cartasIguales, int maxPermitido)
    {
        if (cartasIguales.Any(c => c.Subtypes.Contains("SetUp"))) 
            maxPermitido = 70;
        return maxPermitido;
    }
}

